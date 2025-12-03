using System.Collections.Generic;

namespace AI_Translator_Mobile_App.Services
{
    public static class ProviderEndpoints
    {
        public static readonly Dictionary<string, string> Endpoints = new Dictionary<string, string>
        {
            { "OpenAI", "https://api.openai.com/v1/chat/completions" },
            { "Claude", "https://api.anthropic.com/v1/messages" },
            { "Mistral", "https://api.mistral.ai/v1/chat/completions" },
            { "Perplexity", "https://api.perplexity.ai/chat/completions" },
            { "Gemini", "https://generativelanguage.googleapis.com/v1/models" },
            { "DeepSeek", "https://api.deepseek.com/v1/chat/completions" },
            { "LLMapi", "https://api.llmapi.com/chat/completions" },
            { "Grok", "https://api.x.ai/v1/chat/completions" },
            { "OpenRouter", "https://openrouter.ai/api/v1/chat/completions" }
        };

        public static string GetEndpoint(string provider)
        {
            return Endpoints.TryGetValue(provider, out var endpoint) ? endpoint : string.Empty;
        }

        public static List<string> GetProviders()
        {
            return new List<string>(Endpoints.Keys);
        }
    }
}
