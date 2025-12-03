using AI_Translator_Mobile_App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AI_Translator_Mobile_App.Services
{
    public static class CustomModelStorage
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "custom_models.json"
        );

        private static List<CustomModel> _cachedModels = null;

        public static List<CustomModel> LoadCustomModels()
        {
            if (_cachedModels != null)
            {
                return _cachedModels;
            }

            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    _cachedModels = JsonSerializer.Deserialize<List<CustomModel>>(json) ?? new List<CustomModel>();
                    return _cachedModels;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading custom models: {ex.Message}");
            }

            _cachedModels = new List<CustomModel>();
            return _cachedModels;
        }

        public static void SaveCustomModels(List<CustomModel> models)
        {
            try
            {
                string json = JsonSerializer.Serialize(models, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(FilePath, json);
                _cachedModels = models;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving custom models: {ex.Message}");
                throw;
            }
        }

        public static void AddCustomModel(CustomModel model)
        {
            var models = LoadCustomModels();
            models.Add(model);
            SaveCustomModels(models);
        }

        public static void RemoveCustomModel(string modelId)
        {
            var models = LoadCustomModels();
            var model = models.FirstOrDefault(m => m.Id == modelId);
            if (model != null)
            {
                models.Remove(model);
                SaveCustomModels(models);
            }
        }

        public static void RemoveCustomModelByKey(string modelKey)
        {
            var models = LoadCustomModels();
            var model = models.FirstOrDefault(m => m.ModelKey == modelKey);
            if (model != null)
            {
                models.Remove(model);
                SaveCustomModels(models);
            }
        }

        public static CustomModel GetCustomModelByKey(string modelKey)
        {
            var models = LoadCustomModels();
            return models.FirstOrDefault(m => m.ModelKey == modelKey);
        }

        public static void ClearCache()
        {
            _cachedModels = null;
        }
    }
}
