using System.Diagnostics;

namespace AI_Translator_Mobile_App
{
    public abstract partial class BaseAIComparisonPage : ContentPage
    {
        #region initialize variables
        protected string system_role_for_AI = "";
        protected string[] AI_answers = new string[5];
        protected Dictionary<int, (string LLM, string Model,string Label)> modelMappings = new Dictionary<int, (string LLM, string Model,string Label)>();
        #endregion

        public BaseAIComparisonPage()
        {
            InitializeComponent();
            SetupModelMappings();
            UpdateModelLabels();
        }

        // Abstract method to be implemented by derived classes
        protected abstract void SetupModelMappings();

        // Updates the model label texts based on the modelMappings
        // Modified UpdateModelLabels method in BaseAIComparisonPage
        protected void UpdateModelLabels()
        {
            if (modelMappings.ContainsKey(1))
                Model1Label.Text = modelMappings[1].Label;

            if (modelMappings.ContainsKey(2))
                Model2Label.Text = modelMappings[2].Label;

            if (modelMappings.ContainsKey(3))
                Model3Label.Text = modelMappings[3].Label;

            if (modelMappings.ContainsKey(4))
                Model4Label.Text = modelMappings[4].Label;

            if (modelMappings.ContainsKey(5))
                Model5Label.Text = modelMappings[5].Label;
        }

        // Abstract method to set the system role prompt
        protected abstract string GetSystemRolePrompt();

        protected virtual async void OnProcessClicked(object sender, EventArgs e)
        {
            All_AI_Chat_Bots.ClearAllConversations();
            if (string.IsNullOrWhiteSpace(InputEntry.Text))
            {
                await DisplayAlert("Error", "Please enter text to process", "OK");
                return;
            }

            // Show loading indicator and disable button while processing
            MainPageLoadingIndicator.IsVisible = true;
            MainPageLoadingIndicator.IsRunning = true;
            ProcessButton.IsEnabled = false;

            try
            {
                // Get the system role from derived class
                system_role_for_AI = GetSystemRolePrompt();

                // Call the AI services in parallel
                var tasks = new Task[5];

                // Start all API calls in parallel
                for (int i = 0; i < 5; i++)
                {
                    int modelNum = i + 1; // To capture value for closure
                    var (llm, model, Label) = modelMappings[modelNum];

                    tasks[i] = Task.Run(async () => {
                        AI_answers[modelNum - 1] = await All_AI_Chat_Bots.AskAI(
                            llm, model, system_role_for_AI, InputEntry.Text.ToString());

                        // Update UI on main thread
                        MainThread.BeginInvokeOnMainThread(() => {
                            switch (modelNum)
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

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Debug.WriteLine($"Error: {ex}");
            }
            finally
            {
                // Hide loading indicator and re-enable button when finished (even if there was an error)
                MainPageLoadingIndicator.IsVisible = false;
                MainPageLoadingIndicator.IsRunning = false;
                ProcessButton.IsEnabled = true;
            }
        }

        protected async void OnFollowUpClicked(object sender, EventArgs e)
        {
            // Get the button that was clicked
            var button = (Button)sender;

            // Get the model number from the CommandParameter
            if (!int.TryParse(button.CommandParameter?.ToString(), out int modelNumber) || modelNumber < 1 || modelNumber > 5)
            {
                await DisplayAlert("Error", "Invalid model selection", "OK");
                return;
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

            // Check if the follow-up question is empty
            if (string.IsNullOrWhiteSpace(followUpEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a follow-up question", "OK");
                return;
            }

            try
            {
                // Get the selected model information from our dictionary
                var (llm, model, Label) = modelMappings[modelNumber];

                // Get the follow-up question
                string followUpQuestion = followUpEntry.Text;

                // Call the AskFollowUp method
                string response = await All_AI_Chat_Bots.AskFollowUp(
                    llm,
                    model,
                    system_role_for_AI,
                    followUpQuestion);

                // Update the output editor with the new response
                outputEditor.Text = response;

                // Store the new response
                AI_answers[modelNumber - 1] = response;

                // Clear the follow-up input
                followUpEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get follow-up response: {ex.Message}", "OK");
                Debug.WriteLine($"Follow-up error: {ex}");
            }
        }
    }
}
