using AI_Translator_Mobile_App.Models;
using Microsoft.Maui.Storage;
using System.Collections.Generic;

namespace AI_Translator_Mobile_App.Services
{
    public static class ModelMigration
    {
        private const string MigrationKey = "ModelsM igrated";

        public static void MigrateBuiltInModels()
        {
            // Check if migration has already been done
            if (Preferences.Get(MigrationKey, false))
            {
                return;
            }

            var builtInModels = new List<CustomModel>
            {
                // OpenAI
                new CustomModel
                {
                    DisplayName = "GPT-5",
                    ModelKey = "gpt-5-2025-08-07",
                    Provider = "OpenAI",
                    Endpoint = ProviderEndpoints.GetEndpoint("OpenAI"),
                    InputCostPer1M = 1.25,
                    OutputCostPer1M = 10.00
                },
                new CustomModel
                {
                    DisplayName = "GPT-5 Mini",
                    ModelKey = "gpt-5-mini-2025-08-07",
                    Provider = "OpenAI",
                    Endpoint = ProviderEndpoints.GetEndpoint("OpenAI"),
                    InputCostPer1M = 0.25,
                    OutputCostPer1M = 2.00
                },
                new CustomModel
                {
                    DisplayName = "GPT-5 Nano",
                    ModelKey = "gpt-5-nano-2025-08-07",
                    Provider = "OpenAI",
                    Endpoint = ProviderEndpoints.GetEndpoint("OpenAI"),
                    InputCostPer1M = 0.05,
                    OutputCostPer1M = 0.40
                },
                new CustomModel
                {
                    DisplayName = "GPT-4.1",
                    ModelKey = "gpt-4.1-2025-04-14",
                    Provider = "OpenAI",
                    Endpoint = ProviderEndpoints.GetEndpoint("OpenAI"),
                    InputCostPer1M = 2.00,
                    OutputCostPer1M = 8.00
                },

                // Google
                new CustomModel
                {
                    DisplayName = "Gemini 2.5 Pro",
                    ModelKey = "models/gemini-2.5-pro",
                    Provider = "Gemini",
                    Endpoint = ProviderEndpoints.GetEndpoint("Gemini"),
                    InputCostPer1M = 1.25,
                    OutputCostPer1M = 10.00
                },
                new CustomModel
                {
                    DisplayName = "Gemini 2.5 Flash",
                    ModelKey = "models/gemini-2.5-flash",
                    Provider = "Gemini",
                    Endpoint = ProviderEndpoints.GetEndpoint("Gemini"),
                    InputCostPer1M = 0.30,
                    OutputCostPer1M = 2.50
                },
                new CustomModel
                {
                    DisplayName = "Gemini 2.5 Flash Lite",
                    ModelKey = "models/gemini-2.5-flash-lite",
                    Provider = "Gemini",
                    Endpoint = ProviderEndpoints.GetEndpoint("Gemini"),
                    InputCostPer1M = 0.10,
                    OutputCostPer1M = 0.40
                },

                // OpenRouter
                new CustomModel
                {
                    DisplayName = "Llama 4 Scout",
                    ModelKey = "meta-llama/llama-4-scout",
                    Provider = "OpenRouter",
                    Endpoint = ProviderEndpoints.GetEndpoint("OpenRouter"),
                    InputCostPer1M = 0.08,
                    OutputCostPer1M = 0.30
                },
                new CustomModel
                {
                    DisplayName = "Llama 4 Maverick",
                    ModelKey = "meta-llama/llama-4-maverick",
                    Provider = "OpenRouter",
                    Endpoint = ProviderEndpoints.GetEndpoint("OpenRouter"),
                    InputCostPer1M = 0.15,
                    OutputCostPer1M = 0.60
                },

                // Anthropic
                new CustomModel
                {
                    DisplayName = "Claude Sonnet 4.5",
                    ModelKey = "claude-sonnet-4-5-20250929",
                    Provider = "Claude",
                    Endpoint = ProviderEndpoints.GetEndpoint("Claude"),
                    InputCostPer1M = 3.00,
                    OutputCostPer1M = 6.00
                },
                new CustomModel
                {
                    DisplayName = "Claude Haiku 3.5",
                    ModelKey = "claude-3-5-haiku-20241022",
                    Provider = "Claude",
                    Endpoint = ProviderEndpoints.GetEndpoint("Claude"),
                    InputCostPer1M = 0.80,
                    OutputCostPer1M = 4.00
                },

                // Perplexity
                new CustomModel
                {
                    DisplayName = "Perplexity Sonar",
                    ModelKey = "sonar",
                    Provider = "Perplexity",
                    Endpoint = ProviderEndpoints.GetEndpoint("Perplexity"),
                    InputCostPer1M = 1.00,
                    OutputCostPer1M = 1.00
                },

                // xAI
                new CustomModel
                {
                    DisplayName = "Grok-4 Fast Reasoning",
                    ModelKey = "grok-4-fast-reasoning",
                    Provider = "Grok",
                    Endpoint = ProviderEndpoints.GetEndpoint("Grok"),
                    InputCostPer1M = 0.20,
                    OutputCostPer1M = 0.50
                },
                new CustomModel
                {
                    DisplayName = "Grok-4 Fast Non-Reasoning",
                    ModelKey = "grok-4-fast-non-reasoning",
                    Provider = "Grok",
                    Endpoint = ProviderEndpoints.GetEndpoint("Grok"),
                    InputCostPer1M = 0.20,
                    OutputCostPer1M = 0.50
                },
                new CustomModel
                {
                    DisplayName = "Grok-4",
                    ModelKey = "grok-4-0709",
                    Provider = "Grok",
                    Endpoint = ProviderEndpoints.GetEndpoint("Grok"),
                    InputCostPer1M = 3.00,
                    OutputCostPer1M = 15.00
                },

                // Translation
                new CustomModel
                {
                    DisplayName = "DeepL",
                    ModelKey = "DeepL",
                    Provider = "TranslationService",
                    Endpoint = "",
                    InputCostPer1M = 0,
                    OutputCostPer1M = 0
                },
                new CustomModel
                {
                    DisplayName = "Google Translate",
                    ModelKey = "Google Translate",
                    Provider = "TranslationService",
                    Endpoint = "",
                    InputCostPer1M = 0,
                    OutputCostPer1M = 0
                }
            };

            // Save all built-in models
            var existingModels = CustomModelStorage.LoadCustomModels();
            foreach (var model in builtInModels)
            {
                // Only add if not already exists
                if (!existingModels.Exists(m => m.ModelKey == model.ModelKey))
                {
                    CustomModelStorage.AddCustomModel(model);
                }
            }

            // Mark migration as complete
            Preferences.Set(MigrationKey, true);
        }
    }
}
