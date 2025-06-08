using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class GrokChat : BaseChatService
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskGrok(
        string AImodel, string systemRole, string userMessage)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {LLMConfiguration.ApiKeys.Grok}");

        // Build messages array
        var messages = new List<object>();
        if (_systemMessages.ContainsKey(AImodel))
        {
            messages.Add(_systemMessages[AImodel]);
        }
        else if (!string.IsNullOrEmpty(systemRole))
        {
            messages.Add(new { role = "system", content = systemRole });
        }

        messages.AddRange(_conversationHistories[AImodel]);
        var userMsg = new { role = "user", content = userMessage };
        messages.Add(userMsg);

        var payload = new
        {
            model = AImodel,
            messages = messages.ToArray(),
            stream = false,
            temperature = 0
        };

        string jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Estimate input tokens
        int inputTokens = EstimateTokenCount(systemRole) + EstimateTokenCount(userMessage);
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            inputTokens += EstimateTokenCount(msg.content.ToString());
        }

        var stopwatch = Stopwatch.StartNew();

        var response = await client.PostAsync(LLMConfiguration.Endpoints.Grok, content);
        var responseString = await response.Content.ReadAsStringAsync();

        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            return ($"Error: {response.StatusCode}, {responseString}", stopwatch.ElapsedMilliseconds, 0);
        }

        using var doc = JsonDocument.Parse(responseString);
        var responseText = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        // Estimate output tokens
        int outputTokens = EstimateTokenCount(responseText);

        // Calculate estimated cost
        decimal estimatedCost = CalculateCost(AImodel, inputTokens, outputTokens);

        // Add to conversation history
        _conversationHistories[AImodel].Add(userMsg);
        _conversationHistories[AImodel].Add(new { role = "assistant", content = responseText });
        TrimConversationHistory(AImodel);

        return (responseText, stopwatch.ElapsedMilliseconds, estimatedCost);
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskGrokFollowUp(
        string AImodel, string systemRole, string followUpQuestion)
    {
        // Implementation similar to AskGrok but with followUpQuestion
        return await AskGrok(AImodel, systemRole, followUpQuestion);
    }

    public static async Task<(string imageUrl, long elapsedMs, double estimatedCost)> GrokGenerateImage(
    string AImodel, string prompt)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-api-key", LLMConfiguration.ApiKeys.Grok);

        var requestBody = new
        {
            model = AImodel,
            prompt = prompt,
            n = 1
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var stopwatch = Stopwatch.StartNew();

        // Call the API - assuming you have the endpoint configured
        var endpoint = "https://api.x.ai/v1/images/generations";
        var response = await client.PostAsync(endpoint, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            return ($"Error: {response.StatusCode}, {responseBody}", stopwatch.ElapsedMilliseconds, 0);
        }

        // Parse the response to extract the image URL
        using var doc = JsonDocument.Parse(responseBody);
        var imageUrl = doc.RootElement
            .GetProperty("data")[0]
            .GetProperty("url")
            .GetString();

        // Calculate cost - $0.07 per image according to xAI docs
        double estimatedCost = 0.07;

        return (imageUrl, stopwatch.ElapsedMilliseconds, estimatedCost);
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> GrokWebSearch(
    string AImodel, string systemRole, string userMessage, string searchMode = "auto")
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {LLMConfiguration.ApiKeys.Grok}");

        // Build messages array
        var messages = new List<object>();
        if (_systemMessages.ContainsKey(AImodel))
        {
            messages.Add(_systemMessages[AImodel]);
        }
        else if (!string.IsNullOrEmpty(systemRole))
        {
            messages.Add(new { role = "system", content = systemRole });
        }

        messages.AddRange(_conversationHistories[AImodel]);
        var userMsg = new { role = "user", content = userMessage };
        messages.Add(userMsg);

        // Create payload with search_parameters for real-time web access
        var payload = new
        {
            model = AImodel,
            messages = messages.ToArray(),
            search_parameters = new { mode = searchMode },
            stream = false,
            temperature = 0
        };

        string jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Estimate input tokens
        int inputTokens = EstimateTokenCount(systemRole) + EstimateTokenCount(userMessage);
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            inputTokens += EstimateTokenCount(msg.content.ToString());
        }

        var stopwatch = Stopwatch.StartNew();

        var response = await client.PostAsync(LLMConfiguration.Endpoints.Grok, content);
        var responseString = await response.Content.ReadAsStringAsync();

        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            return ($"Error: {response.StatusCode}, {responseString}", stopwatch.ElapsedMilliseconds, 0);
        }

        using var doc = JsonDocument.Parse(responseString);
        var responseText = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        // Estimate output tokens (plus small surcharge for web search)
        int outputTokens = EstimateTokenCount(responseText);

        // Calculate estimated cost (potentially higher for web search)
        // Assuming web search costs a bit more based on most LLM pricing models
        decimal estimatedCost = CalculateCost(AImodel, inputTokens, outputTokens) * 1.2m;

        // Add to conversation history
        _conversationHistories[AImodel].Add(userMsg);
        _conversationHistories[AImodel].Add(new { role = "assistant", content = responseText });
        TrimConversationHistory(AImodel);

        return (responseText, stopwatch.ElapsedMilliseconds, estimatedCost);
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> GrokWebSearchFollowUp(
        string AImodel, string systemRole, string followUpQuestion, string searchMode = "auto")
    {
        // For follow-ups, we use the main method but with the follow-up question
        return await GrokWebSearch(AImodel, systemRole, followUpQuestion, searchMode);
    }

}
