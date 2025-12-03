using System;

namespace AI_Translator_Mobile_App.Models
{
    public class CustomModel
    {
        public string Id { get; set; } // Unique identifier
        public string ModelKey { get; set; } // The actual model name/key to use in API calls
        public string DisplayName { get; set; } // What the user sees
        public string Endpoint { get; set; } // API endpoint URL
        public double InputCostPer1M { get; set; } // Cost per 1M input tokens
        public double OutputCostPer1M { get; set; } // Cost per 1M output tokens
        public string Provider { get; set; } // e.g., "OpenAI", "Custom", etc.
        public DateTime CreatedAt { get; set; }

        public CustomModel()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }

        public string GetFormattedDisplayName()
        {
            return $"{DisplayName} ({InputCostPer1M:F2}$ input - {OutputCostPer1M:F2}$ output tokens per 1M)";
        }
    }
}
