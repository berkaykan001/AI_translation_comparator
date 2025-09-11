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

        // Grammar check models
        private readonly Dictionary<int, (string LLM, string Model, string Label)> grammarCheckModels = new Dictionary<int, (string LLM, string Model, string Label)>
    {
        { 1, ("OpenRouter", "google/gemini-2.5-flash-preview", "Google Gemini 2.5 Flash") },
        { 2, ("OpenAI", "gpt-4.1-2025-04-14", "OpenAI GPT-4.1") }
    };

        // Translation AI models
        public static readonly Dictionary<int, (string LLM, string Model, string Label)> TranslationAiModels = new Dictionary<int, (string LLM, string Model, string Label)>
    {
        { 3, ("Claude", "claude-sonnet-4-20250514", "Claude 4 Sonnet") },
        { 4, ("OpenAI", "gpt-4o-2024-08-06", "OpenAI GPT-4o") },
        { 5, ("OpenRouter", "meta-llama/llama-4-maverick", "Meta Llama 4 Maverick") }
    };

        private Dictionary<int, (string LLM, string Model, string Label)> aiModelMappings = TranslationAiModels;

        public TranslationPage()
        {
            InitializeComponent();
            UpdateCurrentMode(PageMode.Translation);
            UpdateLanguageButtonStyles();
        }

        private string GetSelectedLanguage()
        {
            return currentLanguage;
        }

        private void OnLanguageButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == EnglishButton)
            {
                currentLanguage = "English";
            }
            else if (button == FrenchButton)
            {
                currentLanguage = "French";
            }
            else if (button == TurkishButton)
            {
                currentLanguage = "Turkish";
            }
            UpdateLanguageButtonStyles();
        }

        private void UpdateLanguageButtonStyles()
        {
            EnglishButton.BackgroundColor = currentLanguage == "English" ? (Color)Application.Current.Resources["AccentDark"] : (Color)Application.Current.Resources["FrameBackgroundColor"];
            FrenchButton.BackgroundColor = currentLanguage == "French" ? (Color)Application.Current.Resources["AccentDark"] : (Color)Application.Current.Resources["FrameBackgroundColor"];
            TurkishButton.BackgroundColor = currentLanguage == "Turkish" ? (Color)Application.Current.Resources["AccentDark"] : (Color)Application.Current.Resources["FrameBackgroundColor"];
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
                    Model1Label.Text = "DeepL";
                    Model2Label.Text = "Google Translate";
                    Model3Label.Text = aiModelMappings[3].Label;
                    Model4Label.Text = aiModelMappings[4].Label;
                    Model5Label.Text = aiModelMappings[5].Label;
                    InputEntry.Placeholder = "Text to translate";
                    break;

                case PageMode.GrammarCheck:
                    Model1Label.Text = grammarCheckModels[1].Label;
                    Model2Label.Text = grammarCheckModels[2].Label;
                    Model3Label.Text = aiModelMappings[3].Label;
                    Model4Label.Text = aiModelMappings[4].Label;
                    Model5Label.Text = aiModelMappings[5].Label;
                    InputEntry.Placeholder = "Text to check grammar";
                    break;

                case PageMode.UsageAnalysis:
                    Model1Label.Text = grammarCheckModels[1].Label;
                    Model2Label.Text = grammarCheckModels[2].Label;
                    Model3Label.Text = aiModelMappings[3].Label;
                    Model4Label.Text = aiModelMappings[4].Label;
                    Model5Label.Text = aiModelMappings[5].Label;
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
                        break;
                    case PageMode.GrammarCheck:
                    case PageMode.UsageAnalysis:
                        await ProcessGrammarCheck(InputEntry.Text, system_role_for_AI, tasks);
                        break;
                }

                await ProcessAIModels(InputEntry.Text, system_role_for_AI, tasks);

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
            for (int i = 0; i < 2; i++)
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
                        }
                    });
                }));
            }
        }

        private async Task ProcessAIModels(string inputText, string systemRole, List<Task> tasks)
        {
            tasks.Add(Task.Run(async () => {
                var result2 = await All_AI_Chat_Bots.AskAI(
                    aiModelMappings[3].LLM, aiModelMappings[3].Model, systemRole, inputText);
                AI_answers[2] = result2.text;
                MainThread.BeginInvokeOnMainThread(() => OutputModel3.Text = AI_answers[2]);
            }));

            tasks.Add(Task.Run(async () => {
                var result3 = await All_AI_Chat_Bots.AskAI(
                    aiModelMappings[4].LLM, aiModelMappings[4].Model, systemRole, inputText);
                AI_answers[3] = result3.text;
                MainThread.BeginInvokeOnMainThread(() => OutputModel4.Text = AI_answers[3]);
            }));

            tasks.Add(Task.Run(async () => {
                var result4 = await All_AI_Chat_Bots.AskAI(
                    aiModelMappings[5].LLM, aiModelMappings[5].Model, systemRole, inputText);
                AI_answers[4] = result4.text;
                MainThread.BeginInvokeOnMainThread(() => OutputModel5.Text = AI_answers[4]);
            }));
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
            
            if (currentMode == PageMode.GrammarCheck || currentMode == PageMode.UsageAnalysis)
            {
                if (modelNumber <= 2)
                {
                    (llm, model, label) = grammarCheckModels[modelNumber];
                }
                else
                {
                    (llm, model, label) = aiModelMappings[modelNumber];
                }
            }
            else
            {
                await DisplayAlert("Error", "Follow-up questions are not available in translation mode", "OK");
                return;
            }
            
            try
            {
                button.IsEnabled = false;
                
                string response = await All_AI_Chat_Bots.AskFollowUp(llm, model, system_role_for_AI, followUpEntry.Text);
                
                outputEditor.Text = response;
                AI_answers[modelNumber - 1] = response;
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
    }
}