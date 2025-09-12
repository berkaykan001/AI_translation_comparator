using Google.Cloud.Translate.V3;
using Google.Apis.Auth.OAuth2;
using DeepL;
using Google.Api.Gax.ResourceNames;
using System.Text;
using Newtonsoft.Json.Linq;

public static class TranslationService
{
    private static readonly string _googleProjectId = "root-blueprint-447618-n7";
    private static readonly string _deeplAuthKey = "7ac7075a-836d-4638-a2bd-65f0294d4b6f:fx";
    private static readonly string _microsoftTranslatorKey = "BCIXijmdJHsTZ0Y8wDySmwUt5Oo9Pjy6uMf4oDvLZLttKhSxlBvhJQQJ99BEAC5RqLJXJ3w3AAAbACOGfuD6";
    private static readonly string _microsoftTranslatorEndpoint = "https://api.cognitive.microsofttranslator.com";
    private static readonly string _microsoftTranslatorRegion = "westeurope";

    private static TranslationServiceClient _googleClient;
    private static Translator _deeplTranslator;

    // Static constructor - runs once when the class is first accessed
    static TranslationService()
    {
        try
        {
            // Initialize Google client with embedded credentials
            var assembly = typeof(TranslationService).Assembly;

            // Use the correct resource name format: AssemblyName.FileName
            using var stream = assembly.GetManifestResourceStream("AI_Translator_Mobile_App.application_default_credentials.json");

            if (stream != null)
            {
                GoogleCredential credential = GoogleCredential.FromStream(stream);
                TranslationServiceClientBuilder builder = new TranslationServiceClientBuilder { Credential = credential };
                _googleClient = builder.Build();
            }
            else
            {
                // Debug: List all available resources to find the correct name
                var resourceNames = assembly.GetManifestResourceNames();
                System.Diagnostics.Debug.WriteLine("Available embedded resources:");
                foreach (var name in resourceNames)
                {
                    System.Diagnostics.Debug.WriteLine($"- {name}");
                }

                _googleClient = null;
            }

            // Initialize DeepL translator
            _deeplTranslator = new Translator(_deeplAuthKey);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"TranslationService initialization error: {ex.Message}");
            _googleClient = null;
            _deeplTranslator = new Translator(_deeplAuthKey);
        }
    }


    /// <summary>
    /// Translates text using Google Cloud Translate V3, DeepL, or Microsoft Translator
    /// </summary>
    /// <param name="translatorModel">The translation service to use ("google", "deepl", or "microsoft")</param>
    /// <param name="sourceLanguage">Source language code (can be null for auto-detection)</param>
    /// <param name="targetLanguage">Target language code</param>
    /// <param name="textToTranslate">The text to translate</param>
    /// <returns>Translated text</returns>
    public static async Task<string> TranslateAsync(string translatorModel, string sourceLanguage, string targetLanguage, string textToTranslate)
    {
        if (string.IsNullOrEmpty(textToTranslate))
            throw new ArgumentException("Text to translate cannot be empty");

        if (string.IsNullOrEmpty(targetLanguage))
            throw new ArgumentException("Target language cannot be empty");

        switch (translatorModel.ToLower())
        {
            case "google":
                return await TranslateWithGoogleAsync(sourceLanguage, targetLanguage, textToTranslate);
            case "deepl":
                return await TranslateWithDeepLAsync(sourceLanguage, targetLanguage, textToTranslate);
            case "microsoft":
                return await TranslateWithMicrosoftAsync(sourceLanguage, targetLanguage, textToTranslate);
            default:
                throw new ArgumentException($"Unsupported translator model: {translatorModel}. Supported values are 'google', 'deepl', or 'microsoft'");
        }
    }

    public static string GetLanguageCode(string selectedLanguage, string translationService)
    {
        // Dictionary mapping friendly names to API codes
        var languageCodes = new Dictionary<string, (string GoogleCode, string DeepLCode, string MicrosoftCode)>
        {
            ["English"] = ("en", "EN-US", "en"),
            ["French"] = ("fr", "FR", "fr"),
            ["Turkish"] = ("tr", "TR", "tr"),
            ["German"] = ("de", "DE", "de"),
            ["Italian"] = ("it", "IT", "it"),
            ["Spanish"] = ("es", "ES", "es"),
            ["Dutch"] = ("nl", "NL", "nl"),
            ["Russian"] = ("ru", "RU", "ru"),
            ["Mandarin"] = ("zh-CN", "ZH", "zh-Hans"),
            ["Cantonese"] = ("zh-TW", "ZH", "zh-Hant"),
            ["Polish"] = ("pl", "PL", "pl"),
            ["Portugal Portuguese"] = ("pt-PT", "PT-PT", "pt-pt"),
            ["Brazilian Portuguese"] = ("pt-BR", "PT-BR", "pt-br"),
            ["Japanese"] = ("ja", "JA", "ja"),
            ["Korean"] = ("ko", "KO", "ko"),
            ["Ukrainian"] = ("uk", "UK", "uk")
        };

        // Get the appropriate code based on the translation service
        if (languageCodes.TryGetValue(selectedLanguage, out var codes))
        {
            if (translationService.Contains("google"))
                return codes.GoogleCode;
            else if (translationService.Contains("deepl"))
                return codes.DeepLCode;
            else // Microsoft
                return codes.MicrosoftCode;
        }
        else
        {
            // Default to English if language not found
            if (translationService.Contains("google"))
                return "en";
            else if (translationService.Contains("deepl"))
                return "EN-US";
            else // Microsoft
                return "en";
        }
    }

    private static async Task<string> TranslateWithGoogleAsync(string sourceLanguage, string targetLanguage, string textToTranslate)
    {
        if (_googleClient == null)
        {
            throw new InvalidOperationException("Google Translation service is not available. Credentials not found.");
        }

        try
        {
            // Your existing Google translation code...
            var locationName = LocationName.FromProjectLocation(_googleProjectId, "global");

            TranslateTextRequest request = new TranslateTextRequest
            {
                Contents = { textToTranslate },
                TargetLanguageCode = targetLanguage,
                Parent = locationName.ToString(),
                MimeType = "text/plain"
            };

            if (!string.IsNullOrEmpty(sourceLanguage))
            {
                request.SourceLanguageCode = sourceLanguage;
            }

            TranslateTextResponse response = await _googleClient.TranslateTextAsync(request);
            return response.Translations[0].TranslatedText;
        }
        catch (Exception ex)
        {
            throw new Exception($"Google Translation error: {ex.Message}", ex);
        }
    }
    
    private static async Task<string> TranslateWithDeepLAsync(string sourceLanguage, string targetLanguage, string textToTranslate)
    {
        try
        {
            // Perform the translation
            var result = await _deeplTranslator.TranslateTextAsync(
                textToTranslate,
                sourceLanguage, // Will be treated as null for auto-detection if empty
                targetLanguage
            );

            // Return the translated text
            return result.Text;
        }
        catch (Exception ex)
        {
            throw new Exception($"DeepL Translation error: {ex.Message}", ex);
        }
    }

    private static async Task<string> TranslateWithMicrosoftAsync(string sourceLanguage,string targetLanguage,string textToTranslate)
    {
        try
        {
            // Build the route
            string route = "/translate?api-version=3.0";
            if (!string.IsNullOrEmpty(sourceLanguage))
                route += $"&from={sourceLanguage}";
            route += $"&to={targetLanguage}";

            // Request body
            object[] body = new[] { new { Text = textToTranslate } };
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            // Send HTTP request
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, _microsoftTranslatorEndpoint + route)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Ocp-Apim-Subscription-Key", _microsoftTranslatorKey);
            if (!string.IsNullOrEmpty(_microsoftTranslatorRegion))
                request.Headers.Add("Ocp-Apim-Subscription-Region", _microsoftTranslatorRegion);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Parse as JArray, not JObject
            JArray array = JArray.Parse(json);
            // array[0]["translations"][0]["text"]
            return (string)array[0]["translations"]![0]!["text"]!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Microsoft Translator error: {ex.Message}", ex);
        }
    }
}

//bazı linkler:
//https://cloud.google.com/docs/authentication/provide-credentials-adc#how-to google credentials'ı alman gereken yer. (seninki local development environment)
//https://portal.azure.com/#@berkayk94hotmail.onmicrosoft.com/resource/subscriptions/c47722e2-72b0-4672-8eea-a2a04fe4e534/resourceGroups/TranslatorForBeko/providers/Microsoft.CognitiveServices/accounts/TranslatorForC/overview bing için
//https://www.deepl.com/en/pro#developer deepL key alman gereken yer.