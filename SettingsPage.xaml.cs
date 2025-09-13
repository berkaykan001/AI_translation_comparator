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

        private readonly List<string> models = new List<string>
        {
            "gpt-5", "gpt-5-mini", "gpt-4o", "gpt-4o-mini", "gpt-4.1",
            "claude-sonnet-4-20250514", "claude-3-5-haiku-20241022", "claude-opus-4-1-20250805",
            "grok-4",
            "sonar",
            "meta-llama/llama-4-maverick", "meta-llama/llama-4-scout",
            "gemini-2.5-pro", "gemini-2.5-flash",
            "DeepL", "Google Translate"
        };

        private readonly List<string> grammarAndUsageModels = new List<string>
        {
            "gpt-5", "gpt-5-mini", "gpt-4o", "gpt-4o-mini", "gpt-4.1",
            "claude-sonnet-4-20250514", "claude-3-5-haiku-20241022", "claude-opus-4-1-20250805",
            "grok-4",
            "sonar",
            "meta-llama/llama-4-maverick", "meta-llama/llama-4-scout",
            "gemini-2.5-pro", "gemini-2.5-flash"
        };

        private List<Tuple<CheckBox, Label>> languageCheckBoxes = new List<Tuple<CheckBox, Label>>();

        public SettingsPage()
        {
            InitializeComponent();
            PopulateLanguageSelection();
            PopulatePickers();
            LoadSettings();
        }

        private void PopulateLanguageSelection()
        {
            foreach (var lang in languages)
            {
                var checkBox = new CheckBox();
                var label = new Label { Text = lang, VerticalOptions = LayoutOptions.Center };
                var stackLayout = new HorizontalStackLayout { Spacing = 10 };
                stackLayout.Children.Add(checkBox);
                stackLayout.Children.Add(label);

                checkBox.CheckedChanged += OnLanguageCheckBoxChanged;
                languageCheckBoxes.Add(new Tuple<CheckBox, Label>(checkBox, label));
                LanguageFlexLayout.Children.Add(stackLayout);
            }
        }

        private void OnLanguageCheckBoxChanged(object sender, CheckedChangedEventArgs e)
        {
            var selectedCheckBoxes = languageCheckBoxes.Where(cb => cb.Item1.IsChecked).ToList();
            if (selectedCheckBoxes.Count > 3)
            {
                ((CheckBox)sender).IsChecked = false;
                DisplayAlert("Limit Exceeded", "You can select up to 3 languages.", "OK");
            }
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
        }

        private void LoadSettings()
        {
            var selectedLanguages = Preferences.Get("SelectedLanguages", "English,French,Turkish").Split(',').ToList();
            foreach (var cb in languageCheckBoxes)
            {
                if (selectedLanguages.Contains(cb.Item2.Text))
                {
                    cb.Item1.IsChecked = true;
                }
            }

            TranslationModel1Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel1", "gpt-4o"), out var t1) ? t1 : Preferences.Get("TranslationModel1", "gpt-4o");
            TranslationModel2Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel2", "claude-4-sonnet"), out var t2) ? t2 : Preferences.Get("TranslationModel2", "claude-4-sonnet");
            TranslationModel3Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel3", "gemini-2.5-pro"), out var t3) ? t3 : Preferences.Get("TranslationModel3", "gemini-2.5-pro");
            TranslationModel4Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel4", "grok-4"), out var t4) ? t4 : Preferences.Get("TranslationModel4", "grok-4");
            TranslationModel5Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("TranslationModel5", "Llama-4-Maverick-17B-128E-Instruct-FP8"), out var t5) ? t5 : Preferences.Get("TranslationModel5", "Llama-4-Maverick-17B-128E-Instruct-FP8");

            GrammarModel1Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel1", "gpt-4o-mini"), out var g1) ? g1 : Preferences.Get("GrammarModel1", "gpt-4o-mini");
            GrammarModel2Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel2", "claude-3-haiku"), out var g2) ? g2 : Preferences.Get("GrammarModel2", "claude-3-haiku");
            GrammarModel3Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel3", "gemini-2.5-flash"), out var g3) ? g3 : Preferences.Get("GrammarModel3", "gemini-2.5-flash");
            GrammarModel4Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel4", "grok-4"), out var g4) ? g4 : Preferences.Get("GrammarModel4", "grok-4");
            GrammarModel5Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("GrammarModel5", "sonar"), out var g5) ? g5 : Preferences.Get("GrammarModel5", "sonar");

            UsageModel1Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel1", "gpt-4o-mini"), out var u1) ? u1 : Preferences.Get("UsageModel1", "gpt-4o-mini");
            UsageModel2Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel2", "claude-3-haiku"), out var u2) ? u2 : Preferences.Get("UsageModel2", "claude-3-haiku");
            UsageModel3Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel3", "gemini-2.5-flash"), out var u3) ? u3 : Preferences.Get("UsageModel3", "gemini-2.5-flash");
            UsageModel4Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel4", "grok-4"), out var u4) ? u4 : Preferences.Get("UsageModel4", "grok-4");
            UsageModel5Picker.SelectedItem = ModelDisplayNames.DisplayNames.TryGetValue(Preferences.Get("UsageModel5", "sonar"), out var u5) ? u5 : Preferences.Get("UsageModel5", "sonar");

            NativeLanguagePicker.SelectedItem = Preferences.Get("NativeLanguage", "English");
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            var selectedLanguages = languageCheckBoxes.Where(cb => cb.Item1.IsChecked).Select(cb => cb.Item2.Text).ToList();
            if (selectedLanguages.Count > 0)
            {
                Preferences.Set("SelectedLanguages", string.Join(",", selectedLanguages));
            }

            Preferences.Set("TranslationModel1", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (TranslationModel1Picker.SelectedItem as string)).Key);
            Preferences.Set("TranslationModel2", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (TranslationModel2Picker.SelectedItem as string)).Key);
            Preferences.Set("TranslationModel3", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (TranslationModel3Picker.SelectedItem as string)).Key);
            Preferences.Set("TranslationModel4", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (TranslationModel4Picker.SelectedItem as string)).Key);
            Preferences.Set("TranslationModel5", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (TranslationModel5Picker.SelectedItem as string)).Key);

            Preferences.Set("GrammarModel1", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (GrammarModel1Picker.SelectedItem as string)).Key);
            Preferences.Set("GrammarModel2", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (GrammarModel2Picker.SelectedItem as string)).Key);
            Preferences.Set("GrammarModel3", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (GrammarModel3Picker.SelectedItem as string)).Key);
            Preferences.Set("GrammarModel4", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (GrammarModel4Picker.SelectedItem as string)).Key);
            Preferences.Set("GrammarModel5", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (GrammarModel5Picker.SelectedItem as string)).Key);

            Preferences.Set("UsageModel1", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (UsageModel1Picker.SelectedItem as string)).Key);
            Preferences.Set("UsageModel2", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (UsageModel2Picker.SelectedItem as string)).Key);
            Preferences.Set("UsageModel3", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (UsageModel3Picker.SelectedItem as string)).Key);
            Preferences.Set("UsageModel4", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (UsageModel4Picker.SelectedItem as string)).Key);
            Preferences.Set("UsageModel5", ModelDisplayNames.DisplayNames.FirstOrDefault(x => x.Value == (UsageModel5Picker.SelectedItem as string)).Key);

            Preferences.Set("NativeLanguage", NativeLanguagePicker.SelectedItem as string);

            DisplayAlert("Success", "Settings saved", "OK");
        }
    }
}
