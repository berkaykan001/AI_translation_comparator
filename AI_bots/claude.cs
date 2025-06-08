using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class ClaudeChat : BaseChatService
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskClaude(
        string AImodel, string systemRole, string userMessage)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-api-key", LLMConfiguration.ApiKeys.Claude);
        client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        // Create messages array for Claude API
        var messages = new List<object>();

        // Add conversation history
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            messages.Add(new { role = msg.role.ToString(), content = msg.content.ToString() });
        }

        // Add current user message
        messages.Add(new { role = "user", content = userMessage });

        // Build request body
        var requestBody = new
        {
            model = AImodel,
            messages = messages.ToArray(),
            system = _systemMessages.ContainsKey(AImodel)
                ? (_systemMessages[AImodel] as dynamic).content
                : systemRole,
            max_tokens = 1024
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
        var response = await client.PostAsync(LLMConfiguration.Endpoints.Claude, content);
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
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString();

        // Estimate output tokens
        int outputTokens = EstimateTokenCount(responseText);

        // Calculate estimated cost
        decimal estimatedCost = CalculateCost(AImodel, inputTokens, outputTokens);

        // Add the user message and assistant response to the conversation history
        _conversationHistories[AImodel].Add(new { role = "user", content = userMessage });
        _conversationHistories[AImodel].Add(new { role = "assistant", content = responseText });

        // Trim the conversation to the memory size
        TrimConversationHistory(AImodel);

        return (responseText, stopwatch.ElapsedMilliseconds, estimatedCost);
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskClaudeFollowUp(
        string AImodel, string systemRole, string followUpQuestion)
    {
        // Implementation similar to AskClaude but with followUpQuestion
        return await AskClaude(AImodel, systemRole, followUpQuestion);
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskClaudeWebSearch(
    string AImodel, string systemRole, string userMessage, int maxSearches = 5)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-api-key", LLMConfiguration.ApiKeys.Claude);
        client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        // Enhance system role for better web search results
        string enhancedSystemRole = systemRole;
        if (!enhancedSystemRole.Contains("search") && !enhancedSystemRole.Contains("internet"))
        {
            enhancedSystemRole = $"{systemRole} When you need to answer questions about recent events or specific facts, please search the web and provide detailed information with specific facts, figures, and dates when available.";
        }

        // Create messages array for Claude API
        var messages = new List<object>();

        // Add conversation history
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            messages.Add(new { role = msg.role.ToString(), content = msg.content.ToString() });
        }

        // Add current user message
        messages.Add(new { role = "user", content = userMessage });

        // Build request body with tools array for web search
        var requestBody = new
        {
            model = AImodel,
            messages = messages.ToArray(),
            system = _systemMessages.ContainsKey(AImodel)
                ? (_systemMessages[AImodel] as dynamic).content
                : enhancedSystemRole,
            max_tokens = 2048, // Increased token limit for more detailed responses
            tools = new[]
            {
            new
            {
                type = "web_search_20250305",
                name = "web_search",
                max_uses = maxSearches
            }
        }
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Start the stopwatch before sending the request
        var stopwatch = Stopwatch.StartNew();

        // Call the API
        var response = await client.PostAsync(LLMConfiguration.Endpoints.Claude, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        // Stop the stopwatch after receiving the response
        stopwatch.Stop();

        if (!response.IsSuccessStatusCode)
        {
            return ($"Error: {response.StatusCode}, {responseBody}", stopwatch.ElapsedMilliseconds, 0);
        }

        // Parse the response to extract the text content
        using var doc = JsonDocument.Parse(responseBody);
        string responseText;

        try
        {
            // Get all content blocks in the response
            var contentArray = doc.RootElement.GetProperty("content");
            string finalAnswer = null;

            // Extract all text content from blocks
            var allText = new List<string>();
            for (int i = 0; i < contentArray.GetArrayLength(); i++)
            {
                var element = contentArray[i];
                if (element.TryGetProperty("type", out var typeProp) && typeProp.GetString() == "text")
                {
                    allText.Add(element.GetProperty("text").GetString());
                }
            }

            // If we have multiple text blocks, prefer later ones (final answers)
            // But check if the final answer is actually informative
            if (allText.Count > 0)
            {
                finalAnswer = allText[allText.Count - 1].Trim();

                // Check if answer is non-informative (recommending to look elsewhere, etc.)
                if (finalAnswer.Length < 50 &&
                    (finalAnswer.Contains("consult", StringComparison.OrdinalIgnoreCase) ||
                     finalAnswer.Contains("refer", StringComparison.OrdinalIgnoreCase) ||
                     finalAnswer.Contains("check", StringComparison.OrdinalIgnoreCase) ||
                     finalAnswer.StartsWith(".") ||
                     finalAnswer.Contains("source", StringComparison.OrdinalIgnoreCase)))
                {
                    // Try to find a more informative block from the middle of the interaction
                    for (int i = allText.Count - 2; i >= 0; i--)
                    {
                        string text = allText[i].Trim();
                        if (text.Length > finalAnswer.Length * 2 &&
                            !text.Contains("I'll need to search", StringComparison.OrdinalIgnoreCase) &&
                            !text.Contains("Let me find out", StringComparison.OrdinalIgnoreCase))
                        {
                            finalAnswer = text;
                            break;
                        }
                    }

                    // If still uninformative, combine all text
                    if (finalAnswer.Length < 50)
                    {
                        finalAnswer = string.Join("\n\n", allText.Where(t =>
                            !t.Contains("I'll need to search", StringComparison.OrdinalIgnoreCase) &&
                            !t.Contains("Let me find out", StringComparison.OrdinalIgnoreCase)));
                    }
                }
            }

            responseText = finalAnswer ?? "No final answer found in Claude's response.";

            // Clean up any leading dots or other strange artifacts
            responseText = responseText.TrimStart('.', ' ', '\t', '\n').Trim();
        }
        catch (Exception ex)
        {
            responseText = $"Error parsing web search response: {ex.Message}";
        }

        // Estimate output tokens
        int outputTokens = EstimateTokenCount(responseText);

        // Calculate estimated cost
        decimal estimatedCost = CalculateCost(AImodel, 0, outputTokens) * 1.2m;

        // Add the user message and assistant response to the conversation history
        _conversationHistories[AImodel].Add(new { role = "user", content = userMessage });
        _conversationHistories[AImodel].Add(new { role = "assistant", content = responseText });

        // Trim the conversation to the memory size
        TrimConversationHistory(AImodel);

        return (responseText, stopwatch.ElapsedMilliseconds, estimatedCost);
    }
    // aşırı pahalı ve çok yavaş. Büyük ihtimalle internetin altını üstüne getiriyo. Doğru dürüst documentation'ı yok. İlerde bu gelişince daha efficient hale getirirsin.

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskClaudeWebSearchFollowUp(
        string AImodel, string systemRole, string followUpQuestion, int maxSearches = 5)
    {
        // For follow-ups, we use the main method but with the follow-up question
        return await AskClaudeWebSearch(AImodel, systemRole, followUpQuestion, maxSearches);
    }


}
