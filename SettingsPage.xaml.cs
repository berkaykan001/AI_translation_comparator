using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;

namespace AI_Translator_Mobile_App
{
    public partial class SettingsPage : ContentPage
    {
        private readonly List<string> languages = new List<string>
        {
            "English", "French", "Turkish", "Italian", "German", "Spanish", "Dutch", "Russian", "Japanese", "Cantonese", "Mandarin", "Polish", "Portugal Portuguese", "Brazilian Portuguese"
        };

        private readonly List<string> models = new List<string>
        {
            "gpt-5", "gpt-5-mini", "gpt-4o", "gpt-4o-mini", "gpt-4.1",
            "claude-4-sonnet", "claude-3-haiku",
            "grok-4",
            "sonar",
            "llama-4-maverick", "llama-4-scout",
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
            var pickers = new List<Picker>
            {
                TranslationModel1Picker, TranslationModel2Picker, TranslationModel3Picker, TranslationModel4Picker, TranslationModel5Picker,
                GrammarModel1Picker, GrammarModel2Picker, GrammarModel3Picker, GrammarModel4Picker, GrammarModel5Picker,
                UsageModel1Picker, UsageModel2Picker, UsageModel3Picker, UsageModel4Picker, UsageModel5Picker
            };

            foreach (var picker in pickers)
            {
                picker.ItemsSource = models;
            }
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

            TranslationModel1Picker.SelectedItem = Preferences.Get("TranslationModel1", "gpt-4o");
            TranslationModel2Picker.SelectedItem = Preferences.Get("TranslationModel2", "claude-4-sonnet");
            TranslationModel3Picker.SelectedItem = Preferences.Get("TranslationModel3", "gemini-2.5-pro");
            TranslationModel4Picker.SelectedItem = Preferences.Get("TranslationModel4", "grok-4");
            TranslationModel5Picker.SelectedItem = Preferences.Get("TranslationModel5", "llama-4-maverick");

            GrammarModel1Picker.SelectedItem = Preferences.Get("GrammarModel1", "gpt-4o-mini");
            GrammarModel2Picker.SelectedItem = Preferences.Get("GrammarModel2", "claude-3-haiku");
            GrammarModel3Picker.SelectedItem = Preferences.Get("GrammarModel3", "gemini-2.5-flash");
            GrammarModel4Picker.SelectedItem = Preferences.Get("GrammarModel4", "grok-4");
            GrammarModel5Picker.SelectedItem = Preferences.Get("GrammarModel5", "sonar");

            UsageModel1Picker.SelectedItem = Preferences.Get("UsageModel1", "gpt-4o-mini");
            UsageModel2Picker.SelectedItem = Preferences.Get("UsageModel2", "claude-3-haiku");
            UsageModel3Picker.SelectedItem = Preferences.Get("UsageModel3", "gemini-2.5-flash");
            UsageModel4Picker.SelectedItem = Preferences.Get("UsageModel4", "grok-4");
            UsageModel5Picker.SelectedItem = Preferences.Get("UsageModel5", "sonar");
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            var selectedLanguages = languageCheckBoxes.Where(cb => cb.Item1.IsChecked).Select(cb => cb.Item2.Text).ToList();
            if (selectedLanguages.Count > 0)
            {
                Preferences.Set("SelectedLanguages", string.Join(",", selectedLanguages));
            }

            Preferences.Set("TranslationModel1", TranslationModel1Picker.SelectedItem as string);
            Preferences.Set("TranslationModel2", TranslationModel2Picker.SelectedItem as string);
            Preferences.Set("TranslationModel3", TranslationModel3Picker.SelectedItem as string);
            Preferences.Set("TranslationModel4", TranslationModel4Picker.SelectedItem as string);
            Preferences.Set("TranslationModel5", TranslationModel5Picker.SelectedItem as string);

            Preferences.Set("GrammarModel1", GrammarModel1Picker.SelectedItem as string);
            Preferences.Set("GrammarModel2", GrammarModel2Picker.SelectedItem as string);
            Preferences.Set("GrammarModel3", GrammarModel3Picker.SelectedItem as string);
            Preferences.Set("GrammarModel4", GrammarModel4Picker.SelectedItem as string);
            Preferences.Set("GrammarModel5", GrammarModel5Picker.SelectedItem as string);

            Preferences.Set("UsageModel1", UsageModel1Picker.SelectedItem as string);
            Preferences.Set("UsageModel2", UsageModel2Picker.SelectedItem as string);
            Preferences.Set("UsageModel3", UsageModel3Picker.SelectedItem as string);
            Preferences.Set("UsageModel4", UsageModel4Picker.SelectedItem as string);
            Preferences.Set("UsageModel5", UsageModel5Picker.SelectedItem as string);

            DisplayAlert("Success", "Settings saved", "OK");
        }
    }
}
