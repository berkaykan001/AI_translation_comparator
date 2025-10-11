using System.Diagnostics;
using AI_Translator_Mobile_App.AI_bots;

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
        private string currentLanguage = "English";
        private List<string> _targetLanguages = new List<string>();

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
            _targetLanguages = SettingsPage.GetSelectedTargetLanguages();
            if (_targetLanguages.Any())
            {
                currentLanguage = _targetLanguages[0];
            }
            CreateLanguageButtons(_targetLanguages);
            UpdateLanguageButtonStyles();

            // Helper function to get provider and model
            (string LLM, string Model, string Label) GetModelInfo(string preferenceKey, string defaultModel)
            {
                var model = Preferences.Get(preferenceKey, defaultModel);
                var label = ModelDisplayNames.DisplayNames.TryGetValue(model, out var displayName) ? displayName : model;
                if (LLMConfiguration.ModelProviders.TryGetValue(model, out var provider))
                {
                    return (provider, model, label);
                }
                return ("OpenRouter", model, label);
            }

            translationAiModels = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, GetModelInfo("TranslationModel1", "DeepL") },
                { 2, GetModelInfo("TranslationModel2", "Google Translate") },
                { 3, GetModelInfo("TranslationModel3", "claude-3-5-haiku-20241022") },
                { 4, GetModelInfo("TranslationModel4", "gpt-5-2025-08-07") },
                { 5, GetModelInfo("TranslationModel5", "meta-llama/llama-4-maverick") }
            };

            grammarCheckModels = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, GetModelInfo("GrammarModel1", "gpt-5-mini-2025-08-07") },
                { 2, GetModelInfo("GrammarModel2", "claude-3-5-haiku-20241022") },
                { 3, GetModelInfo("GrammarModel3", "models/gemini-2.5-flash") },
                { 4, GetModelInfo("GrammarModel4", "sonar") },
                { 5, GetModelInfo("GrammarModel5", "gpt-5-nano-2025-08-07") }
            };

            usageAnalysisModels = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, GetModelInfo("UsageModel1", "gpt-5-mini-2025-08-07") },
                { 2, GetModelInfo("UsageModel2", "claude-3-5-haiku-20241022") },
                { 3, GetModelInfo("UsageModel3", "models/gemini-2.5-flash") },
                { 4, GetModelInfo("UsageModel4", "sonar") },
                { 5, GetModelInfo("UsageModel5", "gpt-5-nano-2025-08-07") }
            };
        }

        private void CreateLanguageButtons(List<string> languages)
        {
            var languageGrid = (Grid)LanguageSelector.Content;
            languageGrid.Children.Clear();
            languageGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < languages.Count; i++)
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

        private string GetSelectedNativeLanguage()
        {
            return Preferences.Get("NativeLanguage", "English");
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

            ProcessButton.IsEnabled = false;

            try
            {
                if (currentMode == PageMode.GrammarCheck)
                {
                    system_role_for_AI = $"You'll be given a sentence or a phrase. Your ONLY task is to check if the grammar of the given message is correct or not. If it's not correct, explain why. " +
                    $"Given sentence/phrase might be a question, don't get confused and don't try to answer the question. ONLY check the grammar of the sentence. Generate your response only in {GetSelectedNativeLanguage()}, because the user speaks only {GetSelectedNativeLanguage()}.";
                }
                else if (currentMode == PageMode.UsageAnalysis)
                {
                    system_role_for_AI = $"Analyze the given phrase/word for its usage context. Be very brief (1-2 sentences): formality level, frequency of use, and typical situations. Keep it short and practical. Generate your response only in {GetSelectedNativeLanguage()}, because the user speaks only {GetSelectedNativeLanguage()}.";
                }

                var tasks = new List<Task>();

                switch (currentMode)
                {
                    case PageMode.Translation:
                        if (!string.IsNullOrEmpty(currentLanguage))
                        {
                            await ProcessTranslations(InputEntry.Text, currentLanguage, tasks);
                        }
                        else
                        {
                            await DisplayAlert("Error", "No target language selected.", "OK");
                        }
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
                ProcessButton.IsEnabled = true;
            }
        }

        

        private async Task ProcessTranslations(string inputText, string targetLanguage, List<Task> tasks)
        {
            string system_role_for_AI = $"Translate the given message to {targetLanguage}. Do not add any other comments. Only translate. If there is more than one translation, include them all.";
            for (int i = 0; i < 5; i++)
            {
                var (llm, model, Label) = translationAiModels[i + 1];
                int modelIndex = i;
                var outputEditor = (Editor)this.FindByName($"OutputModel{i + 1}");
                var loadingIndicator = (ActivityIndicator)this.FindByName($"Loading{i + 1}");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    loadingIndicator.IsVisible = true;
                    loadingIndicator.IsRunning = true;
                });

                if (llm == "TranslationService")
                {
                    tasks.Add(CreateTranslationTask(model, targetLanguage, inputText, modelIndex, outputEditor, loadingIndicator));
                }
                else
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var result = await All_AI_Chat_Bots.AskAI(llm, model, system_role_for_AI, inputText);
                        AI_answers[modelIndex] = result.text;

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            outputEditor.Text = AI_answers[modelIndex];
                            loadingIndicator.IsVisible = false;
                            loadingIndicator.IsRunning = false;
                        });
                    }));
                }
            }
        }

        private Task CreateTranslationTask(string service, string language, string text, int resultIndex, Editor outputEditor, ActivityIndicator loadingIndicator)
        {
            return Task.Run(async () => {
                try
                {
                    string languageCode = TranslationService.GetLanguageCode(language, service);
                    string serviceName = service;
                    if (service == "Google Translate")
                    {
                        serviceName = "google";
                    }
                    else if (service == "DeepL")
                    {
                        serviceName = "deepl";
                    }
                    AI_answers[resultIndex] = await TranslationService.TranslateAsync(serviceName, null, languageCode, text);
                    MainThread.BeginInvokeOnMainThread(() => outputEditor.Text = AI_answers[resultIndex]);
                }
                finally
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        loadingIndicator.IsVisible = false;
                        loadingIndicator.IsRunning = false;
                    });
                }
            });
        }

        private async Task ProcessGrammarCheck(string inputText, string systemRole, List<Task> tasks)
        {
            for (int i = 0; i < 5; i++)
            {
                var (llm, model, Label) = grammarCheckModels[i + 1];
                int modelIndex = i;
                var outputEditor = (Editor)this.FindByName($"OutputModel{i + 1}");
                var loadingIndicator = (ActivityIndicator)this.FindByName($"Loading{i + 1}");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    loadingIndicator.IsVisible = true;
                    loadingIndicator.IsRunning = true;
                });

                tasks.Add(Task.Run(async () => {
                    var result = await All_AI_Chat_Bots.AskAI(llm, model, systemRole, inputText);
                    AI_answers[modelIndex] = result.text;

                    MainThread.BeginInvokeOnMainThread(() => {
                        outputEditor.Text = AI_answers[modelIndex];
                        loadingIndicator.IsVisible = false;
                        loadingIndicator.IsRunning = false;
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
                var outputEditor = (Editor)this.FindByName($"OutputModel{i + 1}");
                var loadingIndicator = (ActivityIndicator)this.FindByName($"Loading{i + 1}");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    loadingIndicator.IsVisible = true;
                    loadingIndicator.IsRunning = true;
                });

                tasks.Add(Task.Run(async () => {
                    var result = await All_AI_Chat_Bots.AskAI(llm, model, systemRole, inputText);
                    AI_answers[modelIndex] = result.text;

                    MainThread.BeginInvokeOnMainThread(() => {
                        outputEditor.Text = AI_answers[modelIndex];
                        loadingIndicator.IsVisible = false;
                        loadingIndicator.IsRunning = false;
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

        private async void OnOutputTapped(object sender, EventArgs e)
        {
            if (sender is Editor editor && !string.IsNullOrEmpty(editor.Text))
            {
                await Clipboard.SetTextAsync(editor.Text);
                await DisplayAlert("Copied", "The text has been copied to the clipboard.", "OK");
            }
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}