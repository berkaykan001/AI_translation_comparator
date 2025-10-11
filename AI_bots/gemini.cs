using Google.Apis.Auth.OAuth2;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class GeminiChat : BaseChatService
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskGemini(
        string AImodel, string systemRole, string userMessage)
    {
        // Ensure this model has a conversation history
        EnsureConversationExists(AImodel);

        using var client = new HttpClient();

        // Build contents array for the API request in Gemini's expected format
        var contents = new List<object>();

        // Add system message if provided or if exists in system messages
        if (_systemMessages.ContainsKey(AImodel) || !string.IsNullOrEmpty(systemRole))
        {
            string systemPrompt = _systemMessages.ContainsKey(AImodel)
                ? (_systemMessages[AImodel] as dynamic).content
                : systemRole;

            contents.Add(new { role = "user", parts = new[] { new { text = $"{systemPrompt}" } } });
            contents.Add(new { role = "model", parts = new[] { new { text = "I'll follow these instructions carefully." } } });
        }

        // Add conversation history in Gemini format
        foreach (dynamic msg in _conversationHistories[AImodel])
        {
            string role = msg.role == "user" ? "user" : "model";
            contents.Add(new { role = role, parts = new[] { new { text = msg.content.ToString() } } });
        }

        // Add current user message
        contents.Add(new { role = "user", parts = new[] { new { text = userMessage } } });

        var requestBody = new
        {
            contents = contents.ToArray(),
            generationConfig = new
            {
                temperature = 0.7,
                topP = 0.95,
                topK = 40,
                maxOutputTokens = 8192
            }
        };

        var jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
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
        string endpoint = $"https://generativelanguage.googleapis.com/v1/{AImodel}:generateContent?key={LLMConfiguration.ApiKeys.Gemini}";
        var response = await client.PostAsync(endpoint, content);
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
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
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

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskGeminiFollowUp(
        string AImodel, string systemRole, string followUpQuestion)
    {
        // Implementation similar to AskGemini but with followUpQuestion
        return await AskGemini(AImodel, systemRole, followUpQuestion);
    }

    public static async Task<(string imageUrl, long elapsedMs, double estimatedCost)> GeminiGenerateImage(
        string AImodel, string prompt, string size = "1024x1024")
    {
#if WINDOWS
        using var client = new HttpClient();

        try
        {
            // For Windows, use the original file-based credentials approach
            var credentialsPath = @"C:\Users\Master_BME\AppData\Roaming\gcloud\application_default_credentials.json";

            if (!File.Exists(credentialsPath))
            {
                return ("Error: Google credentials file not found on Windows", 0, 0);
            }

            // Use Google Cloud credentials for Imagen API
            GoogleCredential credential = GoogleCredential.FromFile(credentialsPath)
                .CreateScoped("https://www.googleapis.com/auth/cloud-platform");

            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var requestBody = new
            {
                instances = new[]
                {
                new { prompt = prompt }
            },
                parameters = new
                {
                    sampleCount = 1,
                    aspectRatio = size switch
                    {
                        "1024x1024" => "1:1",
                        "1024x1792" => "9:16",
                        "1792x1024" => "16:9",
                        _ => "1:1"
                    },
                    includeRaiReason = false
                    // REMOVED: seed parameter causes conflict with watermarking
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Add authorization header
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var stopwatch = Stopwatch.StartNew();

            // Use Vertex AI Imagen API endpoint
            string projectId = "root-blueprint-447618-n7"; // Your project ID
            string endpoint = $"https://us-central1-aiplatform.googleapis.com/v1/projects/{projectId}/locations/us-central1/publishers/google/models/imagegeneration@006:predict";

            var response = await client.PostAsync(endpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                return ($"Error: {response.StatusCode}, {responseBody}", stopwatch.ElapsedMilliseconds, 0);
            }

            // Parse the response to extract image data
            using var doc = JsonDocument.Parse(responseBody);
            var imageData = doc.RootElement
                .GetProperty("predictions")[0]
                .GetProperty("bytesBase64Encoded")
                .GetString();

            // Convert to local file
            var imageFileName = $"gemini_{Guid.NewGuid()}.png";
            var imagePath = Path.Combine(FileSystem.CacheDirectory, imageFileName);

            byte[] imageBytes = Convert.FromBase64String(imageData);
            await File.WriteAllBytesAsync(imagePath, imageBytes);

            double estimatedCost = 0.04;

            return (imagePath, stopwatch.ElapsedMilliseconds, estimatedCost);
        }
        catch (Exception ex)
        {
            return ($"Windows Error: {ex.Message}", 0, 0);
        }
#else
    // For mobile platforms, return unavailable message
    await Task.Delay(100);
    return ("Gemini image generation is only available on Windows. Please use OpenAI or Grok for image generation on mobile.", 100, 0);
#endif
    }




}
