
using System.Collections.Generic;
using System.Linq;
using AI_Translator_Mobile_App.Services;

namespace AI_Translator_Mobile_App.AI_bots
{
    public static class ModelDisplayNames
    {
        static ModelDisplayNames()
        {
            // Migrate built-in models on first run
            ModelMigration.MigrateBuiltInModels();
        }

        public static Dictionary<string, string> DisplayNames
        {
            get
            {
                var allModels = new Dictionary<string, string>();

                // Load all models from custom storage (includes migrated built-in models)
                var customModels = CustomModelStorage.LoadCustomModels();
                foreach (var customModel in customModels)
                {
                    allModels[customModel.ModelKey] = customModel.GetFormattedDisplayName();
                }

                return allModels;
            }
        }

        public static void RefreshDisplayNames()
        {
            CustomModelStorage.ClearCache();
        }
    }
}
