using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class OpenAIChat : BaseChatService
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskOpenai(
        string AImodel, string systemRole, string userMessage)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {LLMConfiguration.ApiKeys.OpenAI}");

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

        var requestBody = new
        {
            model = AImodel,
            messages = messages.ToArray()
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Estimate input tokens
        int inputTokens = EstimateTokenCount(systemRole) + EstimateTokenCount(userMessage);
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            inputTokens += EstimateTokenCount(msg.content.ToString());
        }

        // Start the stopwatch before sending the request
        var stopwatch = Stopwatch.StartNew();

        // Call the API
        var response = await client.PostAsync(LLMConfiguration.Endpoints.OpenAI, content);
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
        decimal estimatedCost;
        if (AImodel== "gpt-4o-search-preview" || AImodel== "gpt-4o-mini-search-preview")
        {
            estimatedCost = CalculateOpenAIWebSearchCost(AImodel);
        }
        else
        {
            estimatedCost = CalculateCost(AImodel, inputTokens, outputTokens);
        }
        // Add the user message and assistant response to the conversation history
        _conversationHistories[AImodel].Add(userMsg);
        _conversationHistories[AImodel].Add(new { role = "assistant", content = responseText });

        // Trim the conversation to the memory size
        TrimConversationHistory(AImodel);

        return (responseText, stopwatch.ElapsedMilliseconds, estimatedCost);
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskOpenaiFollowUp(
        string AImodel, string systemRole, string followUpQuestion)
    {
        // Implementation similar to AskOpenai but with followUpQuestion
        return await AskOpenai(AImodel, systemRole, followUpQuestion);
    }

    public static async Task<(string imageUrl, long elapsedMs, double estimatedCost)> OpenaiGenerateImage(
        string AImodel, string prompt)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {LLMConfiguration.ApiKeys.OpenAI}");

        var requestBody = new
        {
            model = AImodel,
            prompt = prompt,
            n = 1
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Start the stopwatch before sending the request
        var stopwatch = Stopwatch.StartNew();

        // Call the API
        var response = await client.PostAsync(LLMConfiguration.Endpoints.OpenAIImage, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Stop the stopwatch after receiving the response
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

        // Calculate estimated cost - for images it's per image, not per token
        double estimatedCost=0.0;

        if (AImodel == "dall-e-3")
        { 
            estimatedCost = 0.04;
        }
        else if (AImodel == "dall-e-2")
        { 
            estimatedCost = 0.02;
        }
        return (imageUrl, stopwatch.ElapsedMilliseconds, estimatedCost);
    }
}
