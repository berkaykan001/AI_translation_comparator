using AI_Translator_Mobile_App.AI_bots;
using Microsoft.Maui.Controls;
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

        private readonly List<string> models = ModelDisplayNames.DisplayNames.Keys.ToList();

        private readonly List<string> grammarAndUsageModels = ModelDisplayNames.DisplayNames.Keys
            .Where(k => k != "DeepL" && k != "Google Translate").ToList();

        public SettingsPage()
        {
            InitializeComponent();
            PopulatePickers();
            LoadSettings();
            SetupPickerEventHandlers();
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
    }
}
