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

    // Cost per 1M tokens for each model (input/output)
    public static Dictionary<string, (decimal InputCost, decimal OutputCost)> ModelCosts = new()
{
    // OpenAI
    { "gpt-4.1-2025-04-14", (2m, 8m) },
    { "o4-mini-2025-04-16", (1.1m, 4.4m) },
    { "gpt-4o-2024-08-06", (2.5m, 10m) },
    { "gpt-4o-search-preview", (2.5m, 10m) }, // Bunun hesaplanması çok karışık, BaseChatService.CalculateOpenAIWebSearchCost methodu bunu hesaplıyo.
    { "gpt-4o-mini-search-preview", (0.15m, 0.6m) }, // Bunun hesaplanması çok karışık, BaseChatService.CalculateOpenAIWebSearchCost methodu bunu hesaplıyo.
    { "dall-e-3", (0m, 0.04m) }, // Cost per image (Bunlar hard code olarak pass ediliyo, burdan değil. Buraya gör diye koyuyom)
    { "dall-e-2", (0m, 0.02m) }, // Cost per image (Bunlar hard code olarak pass ediliyo, burdan değil. Buraya gör diye koyuyom)
    
    // Claude
    { "claude-3-7-sonnet-20250219", (3m, 15m) },
    { "claude-sonnet-4-20250514", (3m, 15m) },
    { "claude-3-5-haiku-20241022", (0.8m, 4m) },
    
    // MistralAI
    { "ministral-8b-latest", (0.1m, 0.1m) },
    { "ministral-3b-latest", (0.04m, 0.04m) },
    { "mistral-small-latest", (0.1m, 0.3m) },
    
    // Grok
    { "grok-3-beta", (3m, 15m) },
    { "grok-3-fast-beta", (5m, 25m) },
    { "grok-2-image-1212", (0m, 70m) }, // Cost per image (Bunlar hard code olarak pass ediliyo, burdan değil. Buraya gör diye koyuyom)

    // Perplexity
    { "sonar", (15m, 15m) }, // Bu fiyatlar yanlış, bunun hesaplanması çok karışık, BaseChatService.CalculatePerplexityCost methodu bunu hesaplıyo.
    { "sonar-pro", (15m, 15m) },
    { "sonar-reasoning", (15m, 15m) }, // Using same pricing as sonar 
    
    // OpenRouter/Meta
    { "meta-llama/llama-4-maverick", (0.16m, 0.6m) },
    { "meta-llama/llama-4-scout", (0.08m, 0.3m) },
    
    // OpenRouter/Google
    { "google/gemini-2.5-flash-preview", (0.15m, 0.6m) },
    { "google/gemini-2.5-pro-preview", (1.25m, 10m) },

    // LLMapi models
    { "llama4-maverick", (0.16m, 0.6m) },
    { "llama4-scout", (0.08m, 0.3m) },

    // Gemini
    { "gemini-2.0-flash", (0.1m, 0.4m) },
    { "gemini-2.0-flash-lite", (0.075m, 0.3m) },
    { "gemini-2.0-flash-preview-image-generation", (0m, 0.039m) },
    
    // Default for unknown models
    { "default", (1m, 3m) }
};

    // Approximate tokens per word (for estimation)
    public const double TokensPerWord = 1.3;
}
