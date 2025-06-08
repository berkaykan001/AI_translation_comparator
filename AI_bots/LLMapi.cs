using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class LLMapiChat : BaseChatService
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskLLMapi(
        string AImodel, string systemRole, string userMessage)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {LLMConfiguration.ApiKeys.LLMapi}");
        client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "*/*");

        // Build the messages array
        var messages = new List<object>();

        // Add system message if provided or if exists in system messages
        if (_systemMessages.ContainsKey(AImodel))
        {
            messages.Add(_systemMessages[AImodel]);
        }
        else if (!string.IsNullOrEmpty(systemRole))
        {
            messages.Add(new { role = "system", content = systemRole });
        }

        // Add conversation history
        messages.AddRange(_conversationHistories[AImodel]);

        // Add the new user message
        var userMsg = new { role = "user", content = userMessage };
        messages.Add(userMsg);

        // Create request payload
        var requestData = new
        {
            model = AImodel,
            messages = messages.ToArray()
        };

        var jsonContent = JsonSerializer.Serialize(requestData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Estimate input tokens
        int inputTokens = EstimateTokenCount(systemRole) + EstimateTokenCount(userMessage);
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            inputTokens += EstimateTokenCount(msg.content.ToString());
        }

        // Start measuring latency
        var stopwatch = Stopwatch.StartNew();

        // Call the API
        var response = await client.PostAsync(LLMConfiguration.Endpoints.LLMapi, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            return ($"Error: {response.StatusCode}, {responseBody}", stopwatch.ElapsedMilliseconds, 0);
        }

        // Parse the response JSON
        using var jsonDocument = JsonDocument.Parse(responseBody);
        string responseText = jsonDocument.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;

        // Estimate output tokens
        int outputTokens = EstimateTokenCount(responseText);

        // Calculate estimated cost
        decimal estimatedCost = CalculateCost(AImodel, inputTokens, outputTokens);

        // Add the user message and assistant response to the conversation history
        _conversationHistories[AImodel].Add(userMsg);
        _conversationHistories[AImodel].Add(new { role = "assistant", content = responseText });

        // Trim the conversation to the memory size
        TrimConversationHistory(AImodel);

        return (responseText, stopwatch.ElapsedMilliseconds, estimatedCost);
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskLLMapiFollowUp(
        string AImodel, string systemRole, string followUpQuestion)
    {
        // Implementation similar to AskLLMapi but with followUpQuestion
        return await AskLLMapi(AImodel, systemRole, followUpQuestion);
    }
}
