using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class DeepSeekChat : BaseChatService
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskDeepSeek(
        string AImodel, string systemRole, string userMessage)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {LLMConfiguration.ApiKeys.DeepSeek}");

        // Build messages array for the API request
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

        // Add current user message
        var userMsg = new { role = "user", content = userMessage };
        messages.Add(userMsg);

        var payload = new
        {
            model = AImodel,
            messages = messages.ToArray()
        };

        string jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Estimate input tokens
        int inputTokens = EstimateTokenCount(systemRole) + EstimateTokenCount(userMessage);
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            inputTokens += EstimateTokenCount(msg.content.ToString());
        }

        // Start measuring latency
        var stopwatch = Stopwatch.StartNew();

        // Call the API
        var response = await client.PostAsync(LLMConfiguration.Endpoints.DeepSeek, content);
        var responseString = await response.Content.ReadAsStringAsync();

        // Stop the stopwatch
        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            return ($"Error: {response.StatusCode}, {responseString}", stopwatch.ElapsedMilliseconds, 0);
        }

        // Parse the response
        using var document = JsonDocument.Parse(responseString);
        var responseText = document.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

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

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskDeepSeekFollowUp(
        string AImodel, string systemRole, string followUpQuestion)
    {
        // Implementation similar to AskDeepSeek but with followUpQuestion
        return await AskDeepSeek(AImodel, systemRole, followUpQuestion);
    }
}
