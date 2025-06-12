using System.Diagnostics;

namespace AI_Translator_Mobile_App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            modelMappings = DefaultModelMappings;
        }

        private string system_role_for_AI = "";
        private string[] AI_answers = new string[5];

        // Define models in one central place that can be referenced by other classes
        public static readonly Dictionary<int, (string LLM, string Model, string Label)> DefaultModelMappings = new Dictionary<int, (string LLM, string Model,string Label)>
        {
            { 1, ("", "", "") },
            { 2, ("", "", "") },
            { 3, ("", "", "") },
            { 4, ("", "", "") },
            { 5, ("", "", "") }
        };

        private Dictionary<int, (string LLM, string Model, string Label)> modelMappings;

        // radio button event handler
        void OnActionChanged(object sender, CheckedChangedEventArgs e)
        {
            TargetLanguagePicker.IsEnabled = !GrammarCheckRadioButton.IsChecked;
        }

        private async void OnTranslateClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputEntry.Text))
            {
                await DisplayAlert("Error", "Please enter text to process", "OK");
                return;
            }

            // System role selection
            switch (GrammarCheckRadioButton.IsChecked)
            {
                case true:
                    system_role_for_AI = "";
                    break;
                case false:
                    system_role_for_AI = $"";
                    break;
            }

            await ProcessAIQueries(InputEntry.Text, system_role_for_AI, modelMappings);
        }

        // Shared method to process AI queries
        protected async Task ProcessAIQueries(string inputText, string systemRole, Dictionary<int, (string LLM, string Model, string Label)> models)
        {
            var tasks = new Task[5];

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    int modelIndex = i + 1;
                    var (llm, model, Label) = models[modelIndex];

                    tasks[i] = Task.Run(async () => {
                        AI_answers[modelIndex - 1] = await All_AI_Chat_Bots.AskAI(llm, model, systemRole, inputText);

                        MainThread.BeginInvokeOnMainThread(() => {
                            switch (modelIndex)
                            {
                                case 1: OutputModel1.Text = AI_answers[0]; break;
                                case 2: OutputModel2.Text = AI_answers[1]; break;
                                case 3: OutputModel3.Text = AI_answers[2]; break;
                                case 4: OutputModel4.Text = AI_answers[3]; break;
                                case 5: OutputModel5.Text = AI_answers[4]; break;
                            }
                        });
                    });
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void OnFollowUpClicked(object sender, EventArgs e)
        {
            var result = ProcessFollowUp(sender, modelMappings, AI_answers, system_role_for_AI);
            if (result != null)
                await result;
        }

        // Shared method for follow-up processing
        protected async Task<Task> ProcessFollowUp(object sender, Dictionary<int, (string LLM, string Model,string Label)> models,
                                                string[] answers, string systemRole)
        {
            // Get the button that was clicked
            var button = (Button)sender;

            // Get the model number from the CommandParameter
            if (!int.TryParse(button.CommandParameter?.ToString(), out int modelNumber) || modelNumber < 1 || modelNumber > 5)
            {
                await DisplayAlert("Error", "Invalid model selection", "OK");
                return null;
            }

            // Get the appropriate follow-up input based on the model number
            Entry followUpEntry = null;
            Editor outputEditor = null;

            switch (modelNumber)
            {
                case 1:
                    followUpEntry = FollowUpInput1;
                    outputEditor = OutputModel1;
                    break;
                case 2:
                    followUpEntry = FollowUpInput2;
                    outputEditor = OutputModel2;
                    break;
                case 3:
                    followUpEntry = FollowUpInput3;
                    outputEditor = OutputModel3;
                    break;
                case 4:
                    followUpEntry = FollowUpInput4;
                    outputEditor = OutputModel4;
                    break;
                case 5:
                    followUpEntry = FollowUpInput5;
                    outputEditor = OutputModel5;
                    break;
            }

            if (string.IsNullOrWhiteSpace(followUpEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a follow-up question", "OK");
                return null;
            }

            try
            {
                var (llm, model,Label) = models[modelNumber];

                // Get the follow-up question
                string followUpQuestion = followUpEntry.Text;

                // Call the simplified AskFollowUp method
                string response = await All_AI_Chat_Bots.AskFollowUp(
                    llm, model, systemRole, followUpQuestion);

                // Update the output editor with the new response
                outputEditor.Text = response;

                // Store the new response
                answers[modelNumber - 1] = response;

                // Clear the follow-up input
                followUpEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get follow-up response: {ex.Message}", "OK");
                Debug.WriteLine($"Follow-up error: {ex}");
            }

            return null;
        }
    }

    public partial class TranslationPage : ContentPage
    {
        private string system_role_for_AI = "";
        private string[] AI_answers = new string[5];
        private bool isTranslationMode = true;

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

            // Set default values
            TargetLanguagePicker.SelectedIndex = 1; // "French"
            OnActionChanged(null, default);
            UpdateButtonText();
        }

        // In TranslationPage.xaml.cs, update the OnActionChanged method:

        void OnActionChanged(object sender, CheckedChangedEventArgs e)
        {
            // If grammar check is chosen, we don't need a target language
            isTranslationMode = !GrammarCheckRadioButton.IsChecked;

            UpdateButtonText();

            // Update service and model labels based on mode
            if (isTranslationMode)
            {
                // In translation mode, first 3 are translation services
                Model1Label.Text = "DeepL";
                Model2Label.Text = "Google Translate";

                // Show AI model names for slots 4-5
                Model3Label.Text = aiModelMappings[3].Label;
                Model4Label.Text = aiModelMappings[4].Label;
                Model5Label.Text = aiModelMappings[5].Label;
                InputEntry.Placeholder = "Text to translate";
            }
            else
            {
                // In grammar check mode, all 5 are AI models
                Model1Label.Text = grammarCheckModels[1].Label;
                Model2Label.Text = grammarCheckModels[2].Label;
                Model3Label.Text = aiModelMappings[3].Label;
                Model4Label.Text = aiModelMappings[4].Label;
                Model5Label.Text = aiModelMappings[5].Label;
                InputEntry.Placeholder = "Text to check grammar";
            }

            // Show/hide follow-ups for translation services based on mode
            bool isGrammarCheck = !isTranslationMode;
            FollowUp1Container.IsVisible = isGrammarCheck;
            FollowUp2Container.IsVisible = isGrammarCheck;
            FollowUp3Container.IsVisible = isGrammarCheck;
            FollowUp4Container.IsVisible = isGrammarCheck;
            FollowUp5Container.IsVisible = isGrammarCheck;
            TargetLanguagePicker.IsVisible = !isGrammarCheck;

        }

        private void UpdateButtonText()
        {
            ProcessButton.Text = isTranslationMode ? "Translate" : "Check Grammar";
        }

        private async void OnProcessClicked(object sender, EventArgs e)
        {
            All_AI_Chat_Bots.ClearAllConversations();
            if (string.IsNullOrWhiteSpace(InputEntry.Text))
            {
                await DisplayAlert("Error", "Please enter text to process", "OK");
                return;
            }

            // Show loading indicator and disable button while processing
            TranslatorLoadingIndicator.IsVisible = true;
            TranslatorLoadingIndicator.IsRunning = true;
            ProcessButton.IsEnabled = false;

            try
            {
                // System role selection
                switch (isTranslationMode)
                {
                    case false:
                        system_role_for_AI = "You'll be given a sentence or a phrase. Your ONLY task is to check if the grammar of the given message is correct or not. If it's not correct, explain why. " +
                        "Given sentence/phrase might be a question, don't get confused and don't try to answer the question. ONLY check the grammar of the sentence. Make your explanation only in English";
                        break;
                    case true:
                        system_role_for_AI = $"Translate the given message to {TargetLanguagePicker.SelectedItem}. Do not add any other comments. Only translate. If there is more than one translation, include them all.";
                        break;
                }

                var tasks = new List<Task>();

                if (isTranslationMode)
                {
                    await ProcessTranslationServices(InputEntry.Text, tasks);
                }
                else
                {
                    await ProcessGrammarCheck(InputEntry.Text, system_role_for_AI, tasks);
                }

                // Tasks 4-5: AI models (for both translation and grammar check)
                await ProcessAIModels(InputEntry.Text, system_role_for_AI, tasks);

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Debug.WriteLine($"Error: {ex}");
            }
            finally
            {
                // Hide loading indicator and re-enable button when finished (even if there was an error)
                TranslatorLoadingIndicator.IsVisible = false;
                TranslatorLoadingIndicator.IsRunning = false;
                ProcessButton.IsEnabled = true;
            }
        }

        // Helper method to handle translation services
        private async Task ProcessTranslationServices(string inputText, List<Task> tasks)
        {
            string selectedLanguage = TargetLanguagePicker.SelectedItem?.ToString() ?? "English";

            // Add translation service tasks
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

        // Helper method to handle grammar check
        private async Task ProcessGrammarCheck(string inputText, string systemRole, List<Task> tasks)
        {
            for (int i = 0; i < 2; i++)
            {
                var (llm, model, Label) = grammarCheckModels[i + 1];
                int modelIndex = i;

                tasks.Add(Task.Run(async () => {
                    AI_answers[modelIndex] = await All_AI_Chat_Bots.AskAI(llm, model, systemRole, inputText);

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

        // Helper method to handle AI models
        private async Task ProcessAIModels(string inputText, string systemRole, List<Task> tasks)
        {
            // Add slot 3, 4 and 5 AI models
            tasks.Add(Task.Run(async () => {
                AI_answers[2] = await All_AI_Chat_Bots.AskAI(
                    aiModelMappings[3].LLM, aiModelMappings[3].Model, systemRole, inputText);
                MainThread.BeginInvokeOnMainThread(() => OutputModel3.Text = AI_answers[2]);
            }));

            tasks.Add(Task.Run(async () => {
                AI_answers[3] = await All_AI_Chat_Bots.AskAI(
                    aiModelMappings[4].LLM, aiModelMappings[4].Model, systemRole, inputText);
                MainThread.BeginInvokeOnMainThread(() => OutputModel4.Text = AI_answers[3]);
            }));

            tasks.Add(Task.Run(async () => {
                AI_answers[4] = await All_AI_Chat_Bots.AskAI(
                    aiModelMappings[5].LLM, aiModelMappings[5].Model, systemRole, inputText);
                MainThread.BeginInvokeOnMainThread(() => OutputModel5.Text = AI_answers[4]);
            }));
        }

        private async void OnFollowUpClicked(object sender, EventArgs e)
        {
            // Get the button that was clicked
            var button = (Button)sender;

            // Get the model number from the CommandParameter
            if (!int.TryParse(button.CommandParameter?.ToString(), out int modelNumber) || modelNumber < 1 || modelNumber > 5)
            {
                await DisplayAlert("Error", "Invalid model selection", "OK");
                return;
            }

            // Translation services don't support follow-ups in translation mode
            if (isTranslationMode && modelNumber <= 2)
            {
                await DisplayAlert("Error", "Translation services don't support follow-up questions", "OK");
                return;
            }

            // Standard UI setup for follow-ups - similar to MainPage but with mode-specific logic
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

            try
            {
                string response;
                string followUpQuestion = followUpEntry.Text;

                if (modelNumber <= 3 && !isTranslationMode)
                {
                    // Use grammar check models for slots 1-3 in grammar check mode
                    var (llm, model, Label) = grammarCheckModels[modelNumber];
                    response = await All_AI_Chat_Bots.AskFollowUp(llm, model, system_role_for_AI, followUpQuestion);
                }
                else
                {
                    // Use AI models for slots 4-5
                    var (llm, model, Label) = aiModelMappings[modelNumber];
                    response = await All_AI_Chat_Bots.AskFollowUp(llm, model, system_role_for_AI, followUpQuestion);
                }

                outputEditor.Text = response;
                AI_answers[modelNumber - 1] = response;
                followUpEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get follow-up response: {ex.Message}", "OK");
                Debug.WriteLine($"Follow-up error: {ex}");
            }
        }
    }

    public partial class QuickResponsePage : BaseAIComparisonPage
    {
        public QuickResponsePage() : base()
        {
            ProcessButton.Text = "Get Quick Responses";
        }

        protected override void SetupModelMappings()
        {
            modelMappings = new Dictionary<int, (string LLM, string Model,string Label)>
            {
                { 1, ("MistralAI", "ministral-8b-latest", "Ministral 8B") },
                { 2, ("MistralAI", "ministral-3b-latest", "Ministral 3B") },
                { 3, ("MistralAI", "mistral-small-latest", "Mistral Small") },
                { 4, ("Gemini", "gemini-2.0-flash", "Google Gemini 2.0 Flash") },
                { 5, ("OpenAI", "gpt-4o-2024-08-06", "OpenAI GPT-4o") }
            };
        }

        protected override string GetSystemRolePrompt()
        {
            return "You're a low-latency assistant. Provide brief, concise answers that get straight to the point without unnecessary details or explanations.";
        }
    }

    public partial class ReasoningPage : BaseAIComparisonPage
    {
        public ReasoningPage() : base()
        {
            ProcessButton.Text = "Ask";
        }

        protected override void SetupModelMappings()
        {
            modelMappings = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, ("OpenAI", "gpt-4.1-2025-04-14", "OpenAI GPT-4.1") },
                { 2, ("OpenAI", "gpt-4o-2024-08-06", "OpenAI GPT-4o") },
                { 3, ("Claude", "claude-sonnet-4-20250514", "Claude 4 Sonnet") },
                { 4, ("OpenRouter", "google/gemini-2.5-pro-preview", "Google Gemini 2.5 Pro") },
                { 5, ("Perplexity", "sonar-pro", "Perplexity Sonar Pro") }

            };
        }

        protected override string GetSystemRolePrompt()
        {
            return "Provide short and brief answers, do not include unnecessary or extra information. But make sure your answer is accurate and correct.";
        }
    }

    public partial class InternetSearchPage : BaseAIComparisonPage
    {
        public InternetSearchPage() : base()
        {
            ProcessButton.Text = "Get answers";
        }

        protected override void SetupModelMappings()
        {
            modelMappings = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, ("Perplexity", "sonar-pro", "Perplexity Sonar Pro") },
                { 2, ("Perplexity", "sonar", "Perplexity Sonar") },
                { 3, ("OpenAI", "gpt-4o-search-preview", "OpenAI GPT-4o Search") },
                { 4, ("OpenAI", "gpt-4o-mini-search-preview", "OpenAI GPT-4o Mini-Search") },
                { 5, ("GrokWeb", "grok-3-latest", "Grok 3 Search") },
            };
        }

        protected override string GetSystemRolePrompt()
        {
            return "Make sure that you're giving the correct and accurate responses.";
        }
    }

    public partial class CodingPage : BaseAIComparisonPage
    {
        public CodingPage() : base()
        {
            ProcessButton.Text = "Generate Code";
        }

        protected override void SetupModelMappings()
        {
            modelMappings = new Dictionary<int, (string LLM, string Model, string Label)>
            {
                { 1, ("OpenRouter", "google/gemini-2.5-pro-preview", "Google Gemini 2.5 Pro") },
                { 2, ("Claude", "claude-sonnet-4-20250514", "Claude 4 Sonnet") },
                { 3, ("OpenAI", "o4-mini-2025-04-16", "OpenAI o4-mini") },
                { 4, ("OpenAI", "gpt-4.1-2025-04-14", "OpenAI GPT-4.1") },
                { 5, ("OpenAI", "gpt-4o-2024-08-06", "OpenAI GPT-4o") }
            };
        }

        protected override string GetSystemRolePrompt()
        {
            return "You are a helpful assistant, you will only get programming/coding questions. Make sure you're giving the correct code, don't rush your answer.";
        }
    }

    public partial class GeneratePicturePage : ContentPage
    {
        private string openAIModel = "dall-e-3";
        private string geminiModel = "gemini-2.0-flash-preview-image-generation";
        private string grokModel = "grok-2-image-latest";

        public GeneratePicturePage()
        {
            InitializeComponent();
        }

        private async void OnGenerateClicked(object sender, EventArgs e)
        {
            All_AI_Chat_Bots.ClearAllConversations();
            if (string.IsNullOrWhiteSpace(PromptEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a prompt for image generation", "OK");
                return;
            }

            string prompt = PromptEntry.Text;

            // Show loading indicator
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            GenerateButton.IsEnabled = false;
            GeneratedImagesLayout.IsVisible = true;

            // Set initial status
            OpenAIStatusLabel.Text = "Generating...";
            GeminiStatusLabel.Text = "Generating...";
            GrokStatusLabel.Text = "Generating...";

            // Reset previous images
            OpenAIImage.Source = null;
            GeminiImage.Source = null;
            GrokImage.Source = null;

            try
            {
                // Start all image generation tasks in parallel
                var tasks = new List<Task>
                {
                    GenerateOpenAIImage(prompt),
                    GenerateGeminiImage(prompt),
                    GenerateGrokImage(prompt)
                };

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Image generation error: {ex}");
                await DisplayAlert("Error", $"An error occurred during image generation: {ex.Message}", "OK");
            }
            finally
            {
                // Hide loading indicator
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                GenerateButton.IsEnabled = true;
            }
        }

        private async Task GenerateOpenAIImage(string prompt)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Call the OpenAI image generation method
                // Line 77 fix
                var (imageUrl, genElapsedMs, cost) = await OpenAIChat.OpenaiGenerateImage(openAIModel, prompt);

                stopwatch.Stop();

                // Update the UI on the main thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        OpenAIImage.Source = ImageSource.FromUri(new Uri(imageUrl));
                        OpenAIStatusLabel.Text = $"Picture generated in {genElapsedMs} ms for ${cost}.";
                    }
                    else
                    {
                        OpenAIStatusLabel.Text = "Failed to generate image";
                    }
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OpenAIStatusLabel.Text = $"Error: {ex.Message}";
                });
            }
        }

        private async Task GenerateGeminiImage(string prompt)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Call the Gemini image generation method
                var (imageUrl, genElapsedMs, cost) = await GeminiChat.GeminiGenerateImage(geminiModel, prompt);

                stopwatch.Stop();

                // Update the UI on the main thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        // For local files
                        GeminiImage.Source = ImageSource.FromFile(imageUrl);
                        GeminiStatusLabel.Text = $"Picture generated in {genElapsedMs} ms for ${cost}.";
                    }
                    else
                    {
                        GeminiStatusLabel.Text = "Failed to generate image";
                    }
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    GeminiStatusLabel.Text = $"Error: {ex.Message}";
                });
            }
        }

        private async Task GenerateGrokImage(string prompt)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Call the Grok image generation method
                var (imageUrl, genElapsedMs, cost) = await GrokChat.GrokGenerateImage(grokModel, prompt);

                stopwatch.Stop();

                // Update the UI on the main thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        GrokImage.Source = ImageSource.FromUri(new Uri(imageUrl));
                        GrokStatusLabel.Text = $"Picture generated in {genElapsedMs} ms for ${cost}.";
                    }
                    else
                    {
                        GrokStatusLabel.Text = "Failed to generate image";
                    }
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    GrokStatusLabel.Text = $"Error: {ex.Message}";
                });
            }
        }

    }

    public partial class ChooseModelPage : ContentPage
    {
        private string selectedLLM;
        private string selectedModel;
        private string selectedLabel;
        private string lastResponse;

        // Dictionary to map picker selections to actual model info
        private readonly Dictionary<string, (string LLM, string Model, string Label)> modelMappings = new Dictionary<string, (string LLM, string Model, string Label)>
        { // bunun ilk elemanı picker'daki selected item.
            { "OpenAI GPT-4.1", ("OpenAI", "gpt-4.1-2025-04-14", "OpenAI GPT-4.1") },
            { "OpenAI GPT-4o", ("OpenAI", "gpt-4o-2024-08-06", "OpenAI GPT-4o") },
            { "OpenAI o4-mini", ("OpenAI", "o4-mini-2025-04-16", "OpenAI o4-mini") },
            { "OpenAI GPT-4o Search", ("OpenAI", "gpt-4o-search-preview", "OpenAI GPT-4o Search") },
            { "OpenAI GPT-4o Mini-Search", ("OpenAI", "gpt-4o-mini-search-preview", "OpenAI GPT-4o Mini-Search") },
            { "Claude 4 Sonnet", ("Claude", "claude-sonnet-4-20250514", "Claude 4 Sonnet") },
            { "Claude 3.5 Haiku", ("Claude", "claude-3-5-haiku-20241022", "Claude 3.5 Haiku") },
            { "Mistral Small", ("MistralAI", "mistral-small-latest", "Mistral Small") },
            { "Ministral 3B", ("MistralAI", "ministral-3b-latest", "Ministral 3B") },
            { "Ministral 8B", ("MistralAI", "ministral-8b-latest", "Ministral 8B") },
            { "Perplexity Sonar Pro", ("Perplexity", "sonar-pro", "Perplexity Sonar Pro") },
            { "Perplexity Sonar", ("Perplexity", "sonar", "Perplexity Sonar") },
            { "Google Gemini 2.5 Pro", ("OpenRouter", "google/gemini-2.5-pro-preview", "Google Gemini 2.5 Pro") },
            { "Google Gemini 2.5 Flash", ("OpenRouter", "google/gemini-2.5-flash-preview", "Google Gemini 2.5 Flash") },
            { "DeepSeek V3", ("DeepSeek", "deepseek-chat", "DeepSeek V3") },
            { "Meta Llama 4 Maverick", ("LLMapi", "llama4-maverick", "Meta Llama 4 Maverick") },
            { "Meta Llama 4 Scout", ("LLMapi", "llama4-scout", "Meta Llama 4 Scout") },
            { "Grok 3", ("Grok", "grok-3-latest", "Grok 3") },
            { "Grok 3 Search", ("GrokWeb", "grok-3-latest", "Grok 3 Search") }
        };

        public ChooseModelPage()
        {
            InitializeComponent();

            // Set default selection
            ModelPicker.SelectedIndex = 16;
            UpdateSelectedModel();
        }

        private void OnModelSelected(object sender, EventArgs e)
        {
            UpdateSelectedModel();
        }

        private void UpdateSelectedModel()
        {
            string selectedItem = ModelPicker.SelectedItem?.ToString();

            if (selectedItem != null && modelMappings.TryGetValue(selectedItem, out var modelInfo))
            {
                selectedLLM = modelInfo.LLM;
                selectedModel = modelInfo.Model;
                selectedLabel=modelInfo.Label;
                ModelNameLabel.Text = selectedLabel;
            }
        }

        private async void OnAskClicked(object sender, EventArgs e)
        {
            All_AI_Chat_Bots.ClearAllConversations();
            if (string.IsNullOrWhiteSpace(InputEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a message", "OK");
                return;
            }

            // Show loading indicator
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            AskButton.IsEnabled = false;

            try
            {
                // Call the AI service
                lastResponse = await All_AI_Chat_Bots.AskAI(
                    selectedLLM,
                    selectedModel,
                    InstructionsEntry.Text,
                    InputEntry.Text);

                // Update the output
                OutputEditor.Text = lastResponse;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Debug.WriteLine($"Error: {ex}");
            }
            finally
            {
                // Hide loading indicator
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                AskButton.IsEnabled = true;
            }
        }

        private async void OnFollowUpClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FollowUpInput.Text))
            {
                await DisplayAlert("Error", "Please enter a follow-up question", "OK");
                return;
            }

            // Show loading indicator
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            try
            {
                // Call the follow-up method
                string response = await All_AI_Chat_Bots.AskFollowUp(
                    selectedLLM,
                    selectedModel,
                    InstructionsEntry.Text,
                    FollowUpInput.Text);

                // Update the output
                OutputEditor.Text = response;
                lastResponse = response;

                // Clear the follow-up input
                FollowUpInput.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get follow-up response: {ex.Message}", "OK");
                Debug.WriteLine($"Follow-up error: {ex}");
            }
            finally
            {
                // Hide loading indicator
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            }
        }
    }

}
