public static class LLMConfiguration
{
    // API Keys and Endpoints
    public static class ApiKeys
    {
        public static string OpenAI = "sk-Q9lazlc9ZzPndJ6ibmslymmWTr2TSCPr6sBO-zsu7CT3BlbkFJQHPPHU1vkYW6SxddzgVEDHrYwt9vLzuHI9JeFoIaIA";
        public static string Claude = "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";
        public static string Mistral = "wuj46rHMiOiITq1ZKIb08J8D3ouRikNU";
        public static string Perplexity = "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";
        public static string Gemini = "AIzaSyBu-317QXnHgYmgZ_jUHZRlicnHwKj7P-M";
        public static string DeepSeek = "sk-53192f6f481b4653a54d3e37fcac4611";
        public static string LLMapi = "52a55124-426d-4495-bba0-08928513dcaf";
        public static string Grok = "xai-QSWedgxQMReHgWcOAgqK7PmNbTN3qDx7DwXtzJMADmGjfBOeqTjUsZykFFaoYD4vZjnuOevEW4AvKBCF";
        public static string OpenRouter = "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";
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
        { "gpt-5", "OpenAI" },
        { "gpt-5-mini", "OpenAI" },
        { "gpt-5-nano", "OpenAI" },
        { "gpt-4o", "OpenAI" },
        { "gpt-4o-mini", "OpenAI" },
        { "gpt-4.1", "OpenAI" },
        { "gpt-4", "OpenAI" },
        { "gpt-3.5", "OpenAI" },
        { "dall-e-3", "OpenAI" },
        { "dall-e-2", "OpenAI" },

        // Claude
        { "claude-sonnet-4-20250514", "Claude" },
        { "claude-3-5-haiku-20241022", "Claude" },
        { "claude-opus-4-1-20250805", "Claude" },

        // MistralAI
        { "mistral-large-2", "Mistral" },
        { "codestral", "Mistral" },
        { "mistral-7b", "Mistral" },

        // Grok
        { "grok-4", "Grok" },
        { "grok-3-beta", "Grok" },
        { "grok-3-fast-beta", "Grok" },

        // Perplexity
        { "sonar", "Perplexity" },
        { "sonar-pro", "Perplexity" },
        { "sonar-reasoning", "Perplexity" },

        // Meta (via OpenRouter)
        { "meta-llama/llama-4-maverick", "OpenRouter" },
        { "meta-llama/llama-4-scout", "OpenRouter" },

        // Gemini
        { "gemini-2.5-pro", "Gemini" },
        { "gemini-2.5-flash", "Gemini" },
        { "gemini-1.5-pro", "Gemini" },
        { "gemini-1.5-flash", "Gemini" },

        // Cohere (via OpenRouter)
        { "command-r-plus-08-2024", "OpenRouter" },
        { "command-r-plus-04-2024", "OpenRouter" },
        { "command-r-03-2024", "OpenRouter" },

        // Translation Services
        { "DeepL", "TranslationService" },
        { "Google Translate", "TranslationService" },
    };

    // Cost per 1M tokens for each model (input/output)
    public static Dictionary<string, (decimal InputCost, decimal OutputCost)> ModelCosts = new()
{
    // OpenAI
    { "gpt-5", (1.25m, 10m) },
    { "gpt-5-mini", (0.25m, 2m) },
    { "gpt-5-nano", (0.05m, 0.40m) },
    { "gpt-4o", (2.5m, 10m) },
    { "gpt-4o-mini", (0.15m, 0.6m) },
    { "gpt-4.1", (3m, 12m) },
    { "gpt-4", (27.90m, 55.80m) },
    { "gpt-3.5", (0.47m, 1.40m) },
    { "dall-e-3", (0m, 0.04m) }, // Cost per image
    { "dall-e-2", (0m, 0.02m) }, // Cost per image
    
    // Claude
    { "claude-sonnet-4-20250514", (3m, 15m) },
    { "claude-3-5-haiku-20241022", (0.25m, 1.25m) },
    { "claude-opus-4-1-20250805", (15m, 75m) },
    
    // MistralAI
    { "mistral-large-2", (3m, 9m) },
    { "codestral", (1m, 3m) },
    { "mistral-7b", (0.25m, 0.25m) },
    
    // Grok
    { "grok-4", (3m, 15m) },
    { "grok-3-beta", (3m, 15m) },
    { "grok-3-fast-beta", (5m, 25m) },

    // Perplexity
    { "sonar", (15m, 15m) }, // These prices are incorrect, the calculation is complex, BaseChatService.CalculatePerplexityCost method calculates it.
    { "sonar-pro", (15m, 15m) },
    { "sonar-reasoning", (15m, 15m) }, // Using same pricing as sonar 
    
    // Meta
    { "meta-llama/llama-4-maverick", (0.16m, 0.6m) },
    { "meta-llama/llama-4-scout", (0.08m, 0.3m) },

    // Gemini
    { "gemini-2.5-pro", (1.25m, 10m) },
    { "gemini-2.5-flash", (0.50m, 2.00m) },
    { "gemini-1.5-pro", (3.5m, 7m) },
    { "gemini-1.5-flash", (0.35m, 0.7m) },
    
    // Cohere
    { "command-r-plus-08-2024", (2.5m, 10m) },
    { "command-r-plus-04-2024", (3m, 15m) },
    { "command-r-03-2024", (0.5m, 1.5m) },

    // Translation Services
    { "DeepL", (0m, 0m) },
    { "Google Translate", (0m, 0m) },

    // Default for unknown models
    { "default", (1m, 3m) }
};

    // Approximate tokens per word (for estimation)
    public const double TokensPerWord = 1.3;
}