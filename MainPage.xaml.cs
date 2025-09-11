using System.Diagnostics;

namespace AI_Translator_Mobile_App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }

    public enum PageMode
    {
        Translation,
        GrammarCheck,
        UsageAnalysis
    }

    public partial class TranslationPage : ContentPage
    {
        private string system_role_for_AI = "";
        private string[] AI_answers = new string[5];
        private PageMode currentMode = PageMode.Translation;
        private string currentLanguage = "French";

        private Dictionary<int, (string LLM, string Model, string Label)> grammarCheckModels = new Dictionary<int, (string LLM, string Model, string Label)>();
        private Dictionary<int, (string LLM, string Model, string Label)> translationAiModels = new Dictionary<int, (string LLM, string Model, string Label)>();
        private Dictionary<int, (string LLM, string Model, string Label)> usageAnalysisModels = new Dictionary<int, (string LLM, string Model, string Label)>();

        public TranslationPage()
        {
            InitializeComponent();
            LoadSettings();
            UpdateCurrentMode(PageMode.Translation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadSettings();
            UpdateCurrentMode(currentMode);
        }

        private void LoadSettings()
        {
            var selectedLanguages = Preferences.Get("SelectedLanguages", "English,French,Turkish").Split(',');
            currentLanguage = selectedLanguages[0];
            CreateLanguageButtons(selectedLanguages);

            translationAiModels = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, ("DeepL", "", "DeepL") },
                { 2, ("Google", "", "Google Translate") },
                { 3, ("Claude", Preferences.Get("TranslationModel3", "claude-4-sonnet"), "Claude Sonnet 4") },
                { 4, ("OpenAI", Preferences.Get("TranslationModel4", "gpt-4o"), "OpenAI GPT-4o") },
                { 5, ("Gemini", Preferences.Get("TranslationModel5", "gemini-2.5-pro"), "Gemini 2.5 Pro") }
            };

            grammarCheckModels = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, ("OpenAI", Preferences.Get("GrammarModel1", "gpt-4o-mini"), "OpenAI GPT-4o mini") },
                { 2, ("Claude", Preferences.Get("GrammarModel2", "claude-3-haiku"), "Claude Haiku 3") },
                { 3, ("Gemini", Preferences.Get("GrammarModel3", "gemini-2.5-flash"), "Gemini 2.5 Flash") },
                { 4, ("Grok", Preferences.Get("GrammarModel4", "grok-4"), "Grok 4") },
                { 5, ("Perplexity", Preferences.Get("GrammarModel5", "sonar"), "Sonar") }
            };

            usageAnalysisModels = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, ("OpenAI", Preferences.Get("UsageModel1", "gpt-4o-mini"), "OpenAI GPT-4o mini") },
                { 2, ("Claude", Preferences.Get("UsageModel2", "claude-3-haiku"), "Claude Haiku 3") },
                { 3, ("Gemini", Preferences.Get("UsageModel3", "gemini-2.5-flash"), "Gemini 2.5 Flash") },
                { 4, ("Grok", Preferences.Get("UsageModel4", "grok-4"), "Grok 4") },
                { 5, ("Perplexity", Preferences.Get("UsageModel5", "sonar"), "Sonar") }
            };
        }

        private void CreateLanguageButtons(string[] languages)
        {
            var languageGrid = (Grid)LanguageSelector.Content;
            languageGrid.Children.Clear();
            languageGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < languages.Length; i++)
            {
                languageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                var button = new Button
                {
                    Text = languages[i],
                    Style = (Style)Application.Current.Resources["SegmentedButton"]
                };
                button.Clicked += OnLanguageButtonClicked;
                Grid.SetColumn(button, i);
                languageGrid.Children.Add(button);
            }
            UpdateLanguageButtonStyles();
        }

        private void OnLanguageButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            currentLanguage = button.Text;
            UpdateLanguageButtonStyles();
        }

        private void UpdateLanguageButtonStyles()
        {
            var languageGrid = (Grid)LanguageSelector.Content;
            foreach (var child in languageGrid.Children)
            {
                if (child is Button button)
                {
                    button.BackgroundColor = button.Text == currentLanguage ? (Color)Application.Current.Resources["AccentDark"] : (Color)Application.Current.Resources["FrameBackgroundColor"];
                }
            }
        }

        private string GetSelectedLanguage()
        {
            return currentLanguage;
        }

        private void OnModeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == TranslateButton)
            {
                UpdateCurrentMode(PageMode.Translation);
            }
            else if (button == GrammarCheckButton)
            {
                UpdateCurrentMode(PageMode.GrammarCheck);
            }
            else if (button == UsageAnalysisButton)
            {
                UpdateCurrentMode(PageMode.UsageAnalysis);
            }
        }

        private void UpdateCurrentMode(PageMode newMode)
        {
            currentMode = newMode;

            // Update button styles
            TranslateButton.BackgroundColor = currentMode == PageMode.Translation ? (Color)Application.Current.Resources["AccentDark"] : (Color)Application.Current.Resources["FrameBackgroundColor"];
            GrammarCheckButton.BackgroundColor = currentMode == PageMode.GrammarCheck ? (Color)Application.Current.Resources["AccentDark"] : (Color)Application.Current.Resources["FrameBackgroundColor"];
            UsageAnalysisButton.BackgroundColor = currentMode == PageMode.UsageAnalysis ? (Color)Application.Current.Resources["AccentDark"] : (Color)Application.Current.Resources["FrameBackgroundColor"];

            UpdateButtonText();

            // Update service and model labels based on mode
            switch (currentMode)
            {
                case PageMode.Translation:
                    Model1Label.Text = translationAiModels[1].Label;
                    Model2Label.Text = translationAiModels[2].Label;
                    Model3Label.Text = translationAiModels[3].Label;
                    Model4Label.Text = translationAiModels[4].Label;
                    Model5Label.Text = translationAiModels[5].Label;
                    InputEntry.Placeholder = "Text to translate";
                    break;

                case PageMode.GrammarCheck:
                    Model1Label.Text = grammarCheckModels[1].Label;
                    Model2Label.Text = grammarCheckModels[2].Label;
                    Model3Label.Text = grammarCheckModels[3].Label;
                    Model4Label.Text = grammarCheckModels[4].Label;
                    Model5Label.Text = grammarCheckModels[5].Label;
                    InputEntry.Placeholder = "Text to check grammar";
                    break;

                case PageMode.UsageAnalysis:
                    Model1Label.Text = usageAnalysisModels[1].Label;
                    Model2Label.Text = usageAnalysisModels[2].Label;
                    Model3Label.Text = usageAnalysisModels[3].Label;
                    Model4Label.Text = usageAnalysisModels[4].Label;
                    Model5Label.Text = usageAnalysisModels[5].Label;
                    InputEntry.Placeholder = "Text to analyze usage";
                    break;
            }

            // Show/hide follow-ups and language selection based on mode
            bool showFollowUps = currentMode != PageMode.Translation;
            bool showLanguageSelection = currentMode == PageMode.Translation;

            FollowUp1Container.IsVisible = showFollowUps;
            FollowUp2Container.IsVisible = showFollowUps;
            FollowUp3Container.IsVisible = showFollowUps;
            FollowUp4Container.IsVisible = showFollowUps;
            FollowUp5Container.IsVisible = showFollowUps;

            LanguageSelector.IsVisible = showLanguageSelection;
        }

        private void UpdateButtonText()
        {
            ProcessButton.Text = currentMode switch
            {
                PageMode.Translation => "Translate",
                PageMode.GrammarCheck => "Check Grammar",
                PageMode.UsageAnalysis => "Analyze Usage",
                _ => "Process"
            };
        }

        private async void OnProcessClicked(object sender, EventArgs e)
        {
            All_AI_Chat_Bots.ClearAllConversations();
            if (string.IsNullOrWhiteSpace(InputEntry.Text))
            {
                await DisplayAlert("Error", "Please enter text to process", "OK");
                return;
            }

            TranslatorLoadingIndicator.IsVisible = true;
            TranslatorLoadingIndicator.IsRunning = true;
            ProcessButton.IsEnabled = false;

            try
            {
                switch (currentMode)
                {
                    case PageMode.Translation:
                        system_role_for_AI = $"Translate the given message to {GetSelectedLanguage()}. Do not add any other comments. Only translate. If there is more than one translation, include them all.";
                        break;
                    case PageMode.GrammarCheck:
                        system_role_for_AI = "You'''ll be given a sentence or a phrase. Your ONLY task is to check if the grammar of the given message is correct or not. If it'''s not correct, explain why. " +
                        "Given sentence/phrase might be a question, don'''t get confused and don'''t try to answer the question. ONLY check the grammar of the sentence. Make your explanation only in English";
                        break;
                    case PageMode.UsageAnalysis:
                        system_role_for_AI = "Analyze the given phrase/word for its usage context. Be very brief (1-2 sentences): formality level, frequency of use, and typical situations. Keep it short and practical.";
                        break;
                }

                var tasks = new List<Task>();

                switch (currentMode)
                {
                    case PageMode.Translation:
                        await ProcessTranslationServices(InputEntry.Text, tasks);
                        await ProcessAIModels(InputEntry.Text, system_role_for_AI, tasks);
                        break;
                    case PageMode.GrammarCheck:
                        await ProcessGrammarCheck(InputEntry.Text, system_role_for_AI, tasks);
                        break;
                    case PageMode.UsageAnalysis:
                        await ProcessUsageAnalysis(InputEntry.Text, system_role_for_AI, tasks);
                        break;
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Debug.WriteLine($"Error: {ex}");
            }
            finally
            {
                TranslatorLoadingIndicator.IsVisible = false;
                TranslatorLoadingIndicator.IsRunning = false;
                ProcessButton.IsEnabled = true;
            }
        }

        private async Task ProcessTranslationServices(string inputText, List<Task> tasks)
        {
            string selectedLanguage = GetSelectedLanguage();

            tasks.Add(CreateTranslationTask("DeepL", selectedLanguage, inputText, 0, OutputModel1));
            tasks.Add(CreateTranslationTask("Google", selectedLanguage, inputText, 1, OutputModel2));
        }

        private Task CreateTranslationTask(string service, string language, string text, int resultIndex, Editor outputEditor)
        {
            return Task.Run(async () => {
                string languageCode = TranslationService.GetLanguageCode(language, service);
                string serviceName = service.ToLower().Replace(" ", "");
                AI_answers[resultIndex] = await TranslationService.TranslateAsync(serviceName, null, languageCode, text);
                MainThread.BeginInvokeOnMainThread(() => outputEditor.Text = AI_answers[resultIndex]);
            });
        }

        private async Task ProcessGrammarCheck(string inputText, string systemRole, List<Task> tasks)
        {
            for (int i = 0; i < 5; i++)
            {
                var (llm, model, Label) = grammarCheckModels[i + 1];
                int modelIndex = i;

                tasks.Add(Task.Run(async () => {
                    var result = await All_AI_Chat_Bots.AskAI(llm, model, systemRole, inputText);
                    AI_answers[modelIndex] = result.text;

                    MainThread.BeginInvokeOnMainThread(() => {
                        switch (modelIndex)
                        {
                            case 0: OutputModel1.Text = AI_answers[0]; break;
                            case 1: OutputModel2.Text = AI_answers[1]; break;
                            case 2: OutputModel3.Text = AI_answers[2]; break;
                            case 3: OutputModel4.Text = AI_answers[3]; break;
                            case 4: OutputModel5.Text = AI_answers[4]; break;
                        }
                    });
                }));
            }
        }

        private async Task ProcessUsageAnalysis(string inputText, string systemRole, List<Task> tasks)
        {
            for (int i = 0; i < 5; i++)
            {
                var (llm, model, Label) = usageAnalysisModels[i + 1];
                int modelIndex = i;

                tasks.Add(Task.Run(async () => {
                    var result = await All_AI_Chat_Bots.AskAI(llm, model, systemRole, inputText);
                    AI_answers[modelIndex] = result.text;

                    MainThread.BeginInvokeOnMainThread(() => {
                        switch (modelIndex)
                        {
                            case 0: OutputModel1.Text = AI_answers[0]; break;
                            case 1: OutputModel2.Text = AI_answers[1]; break;
                            case 2: OutputModel3.Text = AI_answers[2]; break;
                            case 3: OutputModel4.Text = AI_answers[3]; break;
                            case 4: OutputModel5.Text = AI_answers[4]; break;
                        }
                    });
                }));
            }
        }

        private async Task ProcessAIModels(string inputText, string systemRole, List<Task> tasks)
        {
            for (int i = 2; i < 5; i++)
            {
                var (llm, model, Label) = translationAiModels[i + 1];
                int modelIndex = i;

                tasks.Add(Task.Run(async () => {
                    var result = await All_AI_Chat_Bots.AskAI(llm, model, systemRole, inputText);
                    AI_answers[modelIndex] = result.text;

                    MainThread.BeginInvokeOnMainThread(() => {
                        switch (modelIndex)
                        {
                            case 2: OutputModel3.Text = AI_answers[2]; break;
                            case 3: OutputModel4.Text = AI_answers[3]; break;
                            case 4: OutputModel5.Text = AI_answers[4]; break;
                        }
                    });
                }));
            }
        }

        private async void OnFollowUpClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            
            if (!int.TryParse(button.CommandParameter?.ToString(), out int modelNumber) || modelNumber < 1 || modelNumber > 5)
            {
                await DisplayAlert("Error", "Invalid model selection", "OK");
                return;
            }
            
            Entry followUpEntry = null;
            Editor outputEditor = null;
            
            switch (modelNumber)
            {
                case 1: followUpEntry = FollowUpInput1; outputEditor = OutputModel1; break;
                case 2: followUpEntry = FollowUpInput2; outputEditor = OutputModel2; break;
                case 3: followUpEntry = FollowUpInput3; outputEditor = OutputModel3; break;
                case 4: followUpEntry = FollowUpInput4; outputEditor = OutputModel4; break;
                case 5: followUpEntry = FollowUpInput5; outputEditor = OutputModel5; break;
            }
            
            if (string.IsNullOrWhiteSpace(followUpEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a follow-up question", "OK");
                return;
            }
            
            string llm, model, label;
            
            if (currentMode == PageMode.GrammarCheck)
            {
                 (llm, model, label) = grammarCheckModels[modelNumber];
            }
            else if (currentMode == PageMode.UsageAnalysis)
            {
                 (llm, model, label) = usageAnalysisModels[modelNumber];
            }
            else
            {
                await DisplayAlert("Error", "Follow-up questions are not available in translation mode", "OK");
                return;
            }
            
            try
            {
                button.IsEnabled = false;
                
                var response = await All_AI_Chat_Bots.AskFollowUp(llm, model, system_role_for_AI, followUpEntry.Text);
                
                outputEditor.Text = response.text;
                AI_answers[modelNumber - 1] = response.text;
                followUpEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Follow-up error: {ex}", "OK");
                Debug.WriteLine($"Follow-up error: {ex}");
            }
            finally
            {
                button.IsEnabled = true;
            }
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}