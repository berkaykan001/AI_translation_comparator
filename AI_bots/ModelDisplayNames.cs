
using System.Collections.Generic;

namespace AI_Translator_Mobile_App.AI_bots
{
    public static class ModelDisplayNames
    {
        public static readonly Dictionary<string, string> DisplayNames = new Dictionary<string, string>
        {
            // OpenAI
            { "gpt-5", "GPT-5" },
            { "gpt-5-mini", "GPT-5 Mini" },
            { "gpt-4o", "GPT-4o" },
            { "gpt-4o-mini", "GPT-4o Mini" },
            { "gpt-4.1", "GPT-4.1" },

            // Anthropic
            { "claude-sonnet-4-20250514", "Claude Sonnet 4" },
            { "claude-3-5-haiku-20241022", "Claude Haiku 3.5" },
            { "claude-opus-4-1-20250805", "Claude Opus 4.1" },
            { "claude-3-haiku", "Claude Haiku 3" },


            // xAI
            { "grok-4", "Grok-4" },

            // Perplexity
            { "sonar", "Perplexity Sonar" },

            // Meta
            { "meta-llama/llama-4-maverick", "Llama 4 Maverick" },
            { "meta-llama/llama-4-scout", "Llama 4 Scout" },

            // Google
            { "gemini-2.5-pro", "Gemini 2.5 Pro" },
            { "gemini-2.5-flash", "Gemini 2.5 Flash" },
            { "gemini-1.5-flash", "Gemini 1.5 Flash" },


            // Translation
            { "DeepL", "DeepL" },
            { "Google Translate", "Google Translate" }
        };
    }
}
