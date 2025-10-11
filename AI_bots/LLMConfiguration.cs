
using System.Text.Json;
using System.IO;
using System.Reflection;

public static class LLMConfiguration
{
    public static class ApiKeys
    {
        public static string OpenAI { get; set; }
        public static string Claude { get; set; }
        public static string Mistral { get; set; }
        public static string Perplexity { get; set; }
        public static string Gemini { get; set; }
        public static string DeepSeek { get; set; }
        public static string LLMapi { get; set; }
        public static string Grok { get; set; }
        public static string OpenRouter { get; set; }
    }

    static LLMConfiguration()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AI_Translator_Mobile_App.secrets.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        var secrets = JsonSerializer.Deserialize<Secrets>(json);
                        if (secrets != null && secrets.ApiKeys != null)
                        {
                            ApiKeys.OpenAI = secrets.ApiKeys.OpenAI;
                            ApiKeys.Claude = secrets.ApiKeys.Claude;
                            ApiKeys.Mistral = secrets.ApiKeys.Mistral;
                            ApiKeys.Perplexity = secrets.ApiKeys.Perplexity;
                            ApiKeys.Gemini = secrets.ApiKeys.Gemini;
                            ApiKeys.DeepSeek = secrets.ApiKeys.DeepSeek;
                            ApiKeys.LLMapi = secrets.ApiKeys.LLMapi;
                            ApiKeys.Grok = secrets.ApiKeys.Grok;
                            ApiKeys.OpenRouter = secrets.ApiKeys.OpenRouter;
                        }
                    }
                }
            }
        }
        catch
        {
            // Ignore errors, API keys will be null
        }
    }

    private class Secrets
    {
        public ApiKeysConfig ApiKeys { get; set; }
    }

    private class ApiKeysConfig
    {
        public string OpenAI { get; set; }
        public string Claude { get; set; }
        public string Mistral { get; set; }
        public string Perplexity { get; set; }
        public string Gemini { get; set; }
        public string DeepSeek { get; set; }
        public string LLMapi { get; set; }
        public string Grok { get; set; }
        public string OpenRouter { get; set; }
    }


    public static class Endpoints
    {
        public static string OpenAI = "https://api.openai.com/v1/chat/completions";
        public static string OpenAIImage = "https://api.openai.com/v1/images/generations";
        public static string Claude = "https://api.anthropic.com/v1/messages";
        public static string Mistral = "https://api.mistral.ai/v1/chat/completions";
        public static string Perplexity = "https://api.perplexity.ai/chat/completions";
        public static string Gemini = "https://generativelanguage.googleapis.com/v1/models";
        public static string GeminiImage = "https://generativelanguage.googleapis.com/v1/models/gemini-pro-vision:generateContent";
        public static string DeepSeek = "https://api.deepseek.com/v1/chat/completions";
        public static string LLMapi = "https://api.llmapi.com/chat/completions";
        public static string Grok = "https://api.x.ai/v1/chat/completions";
        public static string GrokImage = "https://api.x.ai/v1/images/generations";
        public static string OpenRouter = "https://openrouter.ai/api/v1/chat/completions";
    }

    public static Dictionary<string, string> ModelProviders = new()
    {
        // OpenAI
        { "gpt-5-2025-08-07", "OpenAI" },
        { "gpt-5-mini-2025-08-07", "OpenAI" },
        { "gpt-5-nano-2025-08-07", "OpenAI" },
        { "gpt-4.1-2025-04-14", "OpenAI" },

        // Google
        { "models/gemini-2.5-pro", "Gemini" },
        { "models/gemini-2.5-flash", "Gemini" },
        { "models/gemini-2.5-flash-lite", "Gemini" },

        // OpenRouter
        { "meta-llama/llama-4-scout", "OpenRouter" },
        { "meta-llama/llama-4-maverick", "OpenRouter" },

        // Anthropic
        { "claude-sonnet-4-5-20250929", "Claude" },
        { "claude-3-5-haiku-20241022", "Claude" },

        // Perplexity
        { "sonar", "Perplexity" },

        // Grok
        { "grok-4-fast-reasoning", "Grok" },
        { "grok-4-fast-non-reasoning", "Grok" },
        { "grok-4-0709", "Grok" },

        // Translation Services
        { "DeepL", "TranslationService" },
        { "Google Translate", "TranslationService" },
    };

    // Cost per 1M tokens for each model (input/output)
    public static Dictionary<string, (decimal InputCost, decimal OutputCost)> ModelCosts = new()
    {
        // OpenAI
        { "gpt-5-2025-08-07", (1.25m, 10.00m) },
        { "gpt-5-mini-2025-08-07", (0.25m, 2.00m) },
        { "gpt-5-nano-2025-08-07", (0.05m, 0.40m) },
        { "gpt-4.1-2025-04-14", (2.00m, 8.00m) },

        // Google
        { "models/gemini-2.5-pro", (1.25m, 10.00m) },
        { "models/gemini-2.5-flash", (0.30m, 2.50m) },
        { "models/gemini-2.5-flash-lite", (0.10m, 0.40m) },

        // OpenRouter
        { "meta-llama/llama-4-scout", (0.08m, 0.30m) },
        { "meta-llama/llama-4-maverick", (0.15m, 0.60m) },

        // Anthropic
        { "claude-sonnet-4-5-20250929", (3.00m, 6.00m) },
        { "claude-3-5-haiku-20241022", (0.80m, 4.00m) },

        // Perplexity
        { "sonar", (1.00m, 1.00m) },

        // Grok
        { "grok-4-fast-reasoning", (0.20m, 0.50m) },
        { "grok-4-fast-non-reasoning", (0.20m, 0.50m) },
        { "grok-4-0709", (3.00m, 15.00m) },

        // Translation Services
        { "DeepL", (0m, 0m) },
        { "Google Translate", (0m, 0m) },

        // Default for unknown models
        { "default", (1m, 3m) }
    };

    // Approximate tokens per word (for estimation)
    public const double TokensPerWord = 1.3;
}