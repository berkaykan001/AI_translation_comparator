using AI_Translator_Mobile_App.AI_bots;
using AI_Translator_Mobile_App.Models;
using AI_Translator_Mobile_App.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Translator_Mobile_App
{
    public partial class SettingsPage : ContentPage
    {
        private readonly List<string> languages = new List<string>
        {
            "English", "French", "Turkish", "Italian", "German", "Spanish", "Dutch", "Russian", "Japanese", "Cantonese", "Mandarin", "Polish", "Portugal Portuguese", "Brazilian Portuguese", "Korean", "Ukrainian"
        };

        private List<string> models = ModelDisplayNames.DisplayNames.Keys.ToList();

        private List<string> grammarAndUsageModels = ModelDisplayNames.DisplayNames.Keys
            .Where(k => k != "DeepL" && k != "Google Translate").ToList();

        public SettingsPage()
        {
            InitializeComponent();
            PopulateProviderPicker();
            PopulatePickers();
            LoadSettings();
            SetupPickerEventHandlers();
            LoadAllModelsForRemoval();
        }

        private void PopulateProviderPicker()
        {
            ModelProviderPicker.ItemsSource = ProviderEndpoints.GetProviders();
            ModelProviderPicker.SelectedIndex = 0; // Default to first provider
        }

        private void PopulatePickers()
        {
            var translationPickers = new List<Picker>
            {
                TranslationModel1Picker, TranslationModel2Picker, TranslationModel3Picker, TranslationModel4Picker, TranslationModel5Picker
            };

            var grammarPickers = new List<Picker>
            {
                GrammarModel1Picker, GrammarModel2Picker, GrammarModel3Picker, GrammarModel4Picker, GrammarModel5Picker
            };

            var usagePickers = new List<Picker>
            {
                UsageModel1Picker, UsageModel2Picker, UsageModel3Picker, UsageModel4Picker, UsageModel5Picker
            };

            foreach (var picker in translationPickers)
            {
                picker.ItemsSource = models.Select(m => ModelDisplayNames.DisplayNames.TryGetValue(m, out var name) ? name : m).ToList();
            }

            foreach (var picker in grammarPickers)
            {
                picker.ItemsSource = grammarAndUsageModels.Select(m => ModelDisplayNames.DisplayNames.TryGetValue(m, out var name) ? name : m).ToList();
            }

            foreach (var picker in usagePickers)
            {
                picker.ItemsSource = grammarAndUsageModels.Select(m => ModelDisplayNames.DisplayNames.TryGetValue(m, out var name) ? name : m).ToList();
            }

            NativeLanguagePicker.ItemsSource = languages;
            TargetLanguage1Picker.ItemsSource = languages;
            TargetLanguage2Picker.ItemsSource = languages;
            TargetLanguage3Picker.ItemsSource = languages;
        }

        private void LoadSettings()
        {
            TranslationModel1Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel1", "gpt-5-2025-08-07"), out var t1) ? t1 : Preferences.Get("TranslationModel1", "gpt-5-2025-08-07");
            TranslationModel2Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel2", "claude-sonnet-4-5-20250929"), out var t2) ? t2 : Preferences.Get("TranslationModel2", "claude-sonnet-4-5-20250929");
            TranslationModel3Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel3", "models/gemini-2.5-flash"), out var t3) ? t3 : Preferences.Get("TranslationModel3", "models/gemini-2.5-flash");
            TranslationModel4Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel4", "meta-llama/llama-4-maverick"), out var t4) ? t4 : Preferences.Get("TranslationModel4", "meta-llama/llama-4-maverick");
            TranslationModel5Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel5", "DeepL"), out var t5) ? t5 : Preferences.Get("TranslationModel5", "DeepL");

            GrammarModel1Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel1", "gpt-5-mini-2025-08-07"), out var g1) ? g1 : Preferences.Get("GrammarModel1", "gpt-5-mini-2025-08-07");
            GrammarModel2Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel2", "claude-3-5-haiku-20241022"), out var g2) ? g2 : Preferences.Get("GrammarModel2", "claude-3-5-haiku-20241022");
            GrammarModel3Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel3", "models/gemini-2.5-flash"), out var g3) ? g3 : Preferences.Get("GrammarModel3", "models/gemini-2.5-flash");
            GrammarModel4Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel4", "meta-llama/llama-4-scout"), out var g4) ? g4 : Preferences.Get("GrammarModel4", "meta-llama/llama-4-scout");
            GrammarModel5Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel5", "sonar"), out var g5) ? g5 : Preferences.Get("GrammarModel5", "sonar");

            UsageModel1Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel1", "gpt-5-mini-2025-08-07"), out var u1) ? u1 : Preferences.Get("UsageModel1", "gpt-5-mini-2025-08-07");
            UsageModel2Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel2", "claude-3-5-haiku-20241022"), out var u2) ? u2 : Preferences.Get("UsageModel2", "claude-3-5-haiku-20241022");
            UsageModel3Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel3", "models/gemini-2.5-flash"), out var u3) ? u3 : Preferences.Get("UsageModel3", "models/gemini-2.5-flash");
            UsageModel4Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel4", "meta-llama/llama-4-scout"), out var u4) ? u4 : Preferences.Get("UsageModel4", "meta-llama/llama-4-scout");
            UsageModel5Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel5", "sonar"), out var u5) ? u5 : Preferences.Get("UsageModel5", "sonar");

            NativeLanguagePicker.SelectedItem = Preferences.Get("NativeLanguage", "English");
            TargetLanguage1Picker.SelectedItem = Preferences.Get("TargetLanguage1", "English");
            TargetLanguage2Picker.SelectedItem = Preferences.Get("TargetLanguage2", "French");
            TargetLanguage3Picker.SelectedItem = Preferences.Get("TargetLanguage3", "Turkish");
        }

        private void SetupPickerEventHandlers()
        {
            NativeLanguagePicker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            TargetLanguage1Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            TargetLanguage2Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            TargetLanguage3Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;

            TranslationModel1Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            TranslationModel2Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            TranslationModel3Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            TranslationModel4Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            TranslationModel5Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;

            GrammarModel1Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            GrammarModel2Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            GrammarModel3Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            GrammarModel4Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            GrammarModel5Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;

            UsageModel1Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            UsageModel2Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            UsageModel3Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            UsageModel4Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
            UsageModel5Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                string preferenceKey = picker.StyleId; // Use StyleId to store preference key

                // Determine preference key based on picker name
                if (picker == NativeLanguagePicker) preferenceKey = "NativeLanguage";
                else if (picker == TargetLanguage1Picker) preferenceKey = "TargetLanguage1";
                else if (picker == TargetLanguage2Picker) preferenceKey = "TargetLanguage2";
                else if (picker == TargetLanguage3Picker) preferenceKey = "TargetLanguage3";

                else if (picker == TranslationModel1Picker) preferenceKey = "TranslationModel1";
                else if (picker == TranslationModel2Picker) preferenceKey = "TranslationModel2";
                else if (picker == TranslationModel3Picker) preferenceKey = "TranslationModel3";
                else if (picker == TranslationModel4Picker) preferenceKey = "TranslationModel4";
                else if (picker == TranslationModel5Picker) preferenceKey = "TranslationModel5";

                else if (picker == GrammarModel1Picker) preferenceKey = "GrammarModel1";
                else if (picker == GrammarModel2Picker) preferenceKey = "GrammarModel2";
                else if (picker == GrammarModel3Picker) preferenceKey = "GrammarModel3";
                else if (picker == GrammarModel4Picker) preferenceKey = "GrammarModel4";
                else if (picker == GrammarModel5Picker) preferenceKey = "GrammarModel5";

                else if (picker == UsageModel1Picker) preferenceKey = "UsageModel1";
                else if (picker == UsageModel2Picker) preferenceKey = "UsageModel2";
                else if (picker == UsageModel3Picker) preferenceKey = "UsageModel3";
                else if (picker == UsageModel4Picker) preferenceKey = "UsageModel4";
                else if (picker == UsageModel5Picker) preferenceKey = "UsageModel5";


                if (!string.IsNullOrEmpty(preferenceKey) && picker.SelectedItem != null)
                {
                    // For models, we need to convert display name back to key
                    if (preferenceKey.Contains("Model"))
                    {
                        var originalKey = ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (picker.SelectedItem as string)).Key;
                        if (originalKey != null)
                        {
                            Preferences.Set(preferenceKey, originalKey);
                        }
                    }
                    else // For languages, save directly
                    {
                        Preferences.Set(preferenceKey, picker.SelectedItem as string);
                    }
                }
            }
        }

        public static List<string> GetSelectedTargetLanguages()
        {
            List<string> targetLanguages = new List<string>();
            string lang1 = Preferences.Get("TargetLanguage1", "English");
            string lang2 = Preferences.Get("TargetLanguage2", "French");
            string lang3 = Preferences.Get("TargetLanguage3", "Turkish");

            if (!string.IsNullOrEmpty(lang1)) targetLanguages.Add(lang1);
            if (!string.IsNullOrEmpty(lang2)) targetLanguages.Add(lang2);
            if (!string.IsNullOrEmpty(lang3)) targetLanguages.Add(lang3);

            return targetLanguages;
        }

        private void LoadAllModelsForRemoval()
        {
            var allModels = CustomModelStorage.LoadCustomModels();
            RemoveModelPicker.ItemsSource = allModels.Select(m => m.GetFormattedDisplayName()).ToList();
        }

        private async void OnAddModelClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(ModelDisplayNameEntry.Text))
                {
                    await DisplayAlert("Error", "Please enter a display name", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ModelKeyEntry.Text))
                {
                    await DisplayAlert("Error", "Please enter a model key", "OK");
                    return;
                }

                if (ModelProviderPicker.SelectedIndex < 0)
                {
                    await DisplayAlert("Error", "Please select a provider", "OK");
                    return;
                }

                if (!double.TryParse(ModelInputCostEntry.Text, out double inputCost))
                {
                    await DisplayAlert("Error", "Please enter a valid input cost", "OK");
                    return;
                }

                if (!double.TryParse(ModelOutputCostEntry.Text, out double outputCost))
                {
                    await DisplayAlert("Error", "Please enter a valid output cost", "OK");
                    return;
                }

                // Get provider and endpoint
                string provider = ModelProviderPicker.SelectedItem as string;
                string endpoint = ProviderEndpoints.GetEndpoint(provider);

                // Create custom model
                var customModel = new CustomModel
                {
                    DisplayName = ModelDisplayNameEntry.Text.Trim(),
                    ModelKey = ModelKeyEntry.Text.Trim(),
                    Endpoint = endpoint,
                    Provider = provider,
                    InputCostPer1M = inputCost,
                    OutputCostPer1M = outputCost
                };

                // Check if model key already exists
                var existingModels = CustomModelStorage.LoadCustomModels();
                if (existingModels.Any(m => m.ModelKey == customModel.ModelKey))
                {
                    await DisplayAlert("Error", "A model with this key already exists", "OK");
                    return;
                }

                // Save model
                CustomModelStorage.AddCustomModel(customModel);

                // Refresh display names and pickers
                ModelDisplayNames.RefreshDisplayNames();
                RefreshPickers();

                // Clear input fields
                ModelDisplayNameEntry.Text = string.Empty;
                ModelKeyEntry.Text = string.Empty;
                ModelInputCostEntry.Text = string.Empty;
                ModelOutputCostEntry.Text = string.Empty;
                ModelProviderPicker.SelectedIndex = 0;

                // Reload removal picker
                LoadAllModelsForRemoval();

                await DisplayAlert("Success", "Model added successfully!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add model: {ex.Message}", "OK");
            }
        }

        private async void OnRemoveModelClicked(object sender, EventArgs e)
        {
            try
            {
                if (RemoveModelPicker.SelectedIndex < 0)
                {
                    await DisplayAlert("Error", "Please select a model to remove", "OK");
                    return;
                }

                var allModels = CustomModelStorage.LoadCustomModels();
                var selectedModel = allModels[RemoveModelPicker.SelectedIndex];

                bool confirm = await DisplayAlert("Confirm",
                    $"Are you sure you want to remove '{selectedModel.DisplayName}'?",
                    "Yes", "No");

                if (!confirm)
                    return;

                // Remove model
                CustomModelStorage.RemoveCustomModel(selectedModel.Id);

                // Refresh display names and pickers
                ModelDisplayNames.RefreshDisplayNames();
                RefreshPickers();

                // Reload removal picker
                LoadAllModelsForRemoval();

                await DisplayAlert("Success", "Model removed successfully!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to remove model: {ex.Message}", "OK");
            }
        }

        private void RefreshPickers()
        {
            // Reload model lists
            models = ModelDisplayNames.DisplayNames.Keys.ToList();
            grammarAndUsageModels = ModelDisplayNames.DisplayNames.Keys
                .Where(k => k != "DeepL" && k != "Google Translate").ToList();

            // Repopulate pickers
            PopulatePickers();

            // Reload settings to restore selected values
            LoadSettings();
        }
    }
}
