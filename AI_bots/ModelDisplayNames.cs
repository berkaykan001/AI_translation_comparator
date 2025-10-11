
using System.Collections.Generic;

namespace AI_Translator_Mobile_App.AI_bots
{
    public static class ModelDisplayNames
    {
        public static readonly Dictionary<string, string> DisplayNames = new Dictionary<string, string>
        {
            // OpenAI
            { "gpt-5-2025-08-07", "GPT-5 (1.25$ input - 10.00$ output tokens per 1M)" },
            { "gpt-5-mini-2025-08-07", "GPT-5 Mini (0.25$ input - 2.00$ output tokens per 1M)" },
            { "gpt-5-nano-2025-08-07", "GPT-5 Nano (0.05$ input - 0.40$ output tokens per 1M)" },
            { "gpt-4.1-2025-04-14", "GPT-4.1 (2.00$ input - 8.00$ output tokens per 1M)" },

            

            // Google
            { "models/gemini-2.5-pro", "Gemini 2.5 Pro (1.25$ input - 10.00$ output tokens per 1M)" },
            { "models/gemini-2.5-flash", "Gemini 2.5 Flash (0.30$ input - 2.50$ output tokens per 1M)" },
            { "models/gemini-2.5-flash-lite", "Gemini 2.5 Flash Lite (0.10$ input - 0.40$ output tokens per 1M)" },

            // OpenRouter
            { "meta-llama/llama-4-scout", "Llama 4 Scout (0.08$ input - 0.30$ output tokens per 1M)" },
            { "meta-llama/llama-4-maverick", "Llama 4 Maverick (0.15$ input - 0.60$ output tokens per 1M)" },

            // Anthropic
            { "claude-sonnet-4-5-20250929", "Claude Sonnet 4.5 (3.00$ input - 6.00$ output tokens per 1M)" },
            { "claude-3-5-haiku-20241022", "Claude Haiku 3.5 (0.80$ input - 4.00$ output tokens per 1M)" },

            // Perplexity
            { "sonar", "Perplexity Sonar (1.00$ input - 1.00$ output tokens per 1M)" },

            // xAI
            { "grok-4-fast-reasoning", "Grok-4 Fast Reasoning (0.20$ input - 0.50$ output tokens per 1M)" },
            { "grok-4-fast-non-reasoning", "Grok-4 Fast Non-Reasoning (0.20$ input - 0.50$ output tokens per 1M)" },
            { "grok-4-0709", "Grok-4 (3.00$ input - 15.00$ output tokens per 1M)" },

            // Translation
            { "DeepL", "DeepL" },
            { "Google Translate", "Google Translate" }
        };
    }
}
