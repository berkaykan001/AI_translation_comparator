using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class PerplexityChat : BaseChatService
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskPerplexity(
    string AImodel, string systemRole, string userMessage)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {LLMConfiguration.ApiKeys.Perplexity}");

        // Build messages array for the API request
        var messages = new List<object>();

        // Add conversation history
        messages.AddRange(_conversationHistories[AImodel]);

        // Embed system role instructions into user message if provided
        string finalUserMessage = userMessage;
        if (!string.IsNullOrEmpty(systemRole))
        {
            finalUserMessage = $"(Instruction: {systemRole}) {userMessage}";
        }

        // Add current user message with embedded system instructions
        var userMsg = new { role = "user", content = finalUserMessage };
        messages.Add(userMsg);

        var requestBody = new
        {
            model = AImodel,
            messages = messages.ToArray()
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Estimate input tokens
        int inputTokens = EstimateTokenCount(finalUserMessage);
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            inputTokens += EstimateTokenCount(msg.content.ToString());
        }

        // Start the stopwatch before sending the request
        var stopwatch = Stopwatch.StartNew();

        // Call the API
        var response = await httpClient.PostAsync(LLMConfiguration.Endpoints.Perplexity, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Stop the stopwatch after receiving the response
        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            return ($"Error: {response.StatusCode}, {responseBody}", stopwatch.ElapsedMilliseconds, 0);
        }

        // Parse the response to extract the text content
        using var doc = JsonDocument.Parse(responseBody);
        var responseText = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        // Estimate output tokens
        int outputTokens = EstimateTokenCount(responseText);

        // Calculate estimated cost
        decimal estimatedCost = CalculatePerplexityCost(AImodel, inputTokens, outputTokens);

        // Add the user message and assistant response to the conversation history
        _conversationHistories[AImodel].Add(userMsg);
        _conversationHistories[AImodel].Add(new { role = "assistant", content = responseText });

        // Trim the conversation to the memory size
        TrimConversationHistory(AImodel);

        return (responseText, stopwatch.ElapsedMilliseconds, estimatedCost);
    }


    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskPerplexityFollowUp(
        string AImodel, string systemRole, string followUpQuestion)
    {
        // Implementation similar to AskPerplexity but with followUpQuestion
        return await AskPerplexity(AImodel, systemRole, followUpQuestion);
    }
}
