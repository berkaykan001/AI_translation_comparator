public abstract class BaseChatService
{
    // Dictionary to store conversation histories for different model IDs
    protected static Dictionary<string, List<object>> _conversationHistories = new();

    // Dictionary to store system messages for different model IDs
    protected static Dictionary<string, object> _systemMessages = new();

    // Default memory size
    protected static int _memorySize = 10;

    // Method to initialize conversation for a model if it doesn't exist
    protected static void EnsureConversationExists(string modelId)
    {
        if (!_conversationHistories.ContainsKey(modelId))
        {
            _conversationHistories[modelId] = new List<object>();
        }
    }

    // Add a permanent system message for a specific model
    public static void AddPermanentSystemMessage(string modelId, string message)
    {
        EnsureConversationExists(modelId);
        _systemMessages[modelId] = new { role = "system", content = message };
    }

    // Set memory size for conversation history
    public static void SetMemorySize(int size)
    {
        if (size > 0)
        {
            _memorySize = size;
            // Trim all existing conversations
            foreach (var modelId in _conversationHistories.Keys.ToList())
            {
                TrimConversationHistory(modelId);
            }
        }
    }

    // Trim conversation history to memory size
    protected static void TrimConversationHistory(string modelId)
    {
        EnsureConversationExists(modelId);

        if (_conversationHistories[modelId].Count > _memorySize * 2) // Multiply by 2 because each exchange has user + assistant messages
        {
            _conversationHistories[modelId] = _conversationHistories[modelId]
                .Skip(_conversationHistories[modelId].Count - _memorySize * 2)
                .ToList();
        }
    }

    // Clear conversation history for a specific model
    public static void ClearConversationHistory(string modelId)
    {
        EnsureConversationExists(modelId);
        _conversationHistories[modelId].Clear();
    }

    // Clear all conversation histories
    public static void ClearAllConversationHistories()
    {
        foreach (var modelId in _conversationHistories.Keys.ToList())
        {
            _conversationHistories[modelId].Clear();
        }
    }

    // Estimate token count from text
    protected static int EstimateTokenCount(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;

        // Count words and multiply by tokens per word factor
        int wordCount = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return (int)(wordCount * LLMConfiguration.TokensPerWord);
    }

    // Calculate estimated cost
    protected static decimal CalculateCost(string modelId, int inputTokens, int outputTokens)
    {
        if (!LLMConfiguration.ModelCosts.TryGetValue(modelId, out var costs))
        {
            costs = LLMConfiguration.ModelCosts["default"];
        }

        decimal inputCost = (inputTokens / 1000000m) * costs.InputCost;
        decimal outputCost = (outputTokens / 1000000m) * costs.OutputCost;

        return inputCost + outputCost;
    }

    // Calculate Perplexity cost for a single execution
    protected static decimal CalculatePerplexityCost(string modelId, int inputTokens, int outputTokens, int searchQueries = 1)
    {
        decimal inputCostPerMillion = 0;
        decimal outputCostPerMillion = 0;
        decimal costPer1000Requests = 0;
        decimal inferenceCostPerMillion = 0;

        // Set pricing based on model
        switch (modelId)
        {
            case "sonar":
                inputCostPerMillion = 1m;
                outputCostPerMillion = 1m;
                costPer1000Requests = 5m; // Using the low tier pricing
                break;
            case "sonar-pro":
                inputCostPerMillion = 3m;
                outputCostPerMillion = 15m;
                costPer1000Requests = 6m; // Using the low tier pricing
                break;
            case "sonar-reasoning":
                inputCostPerMillion = 1m;
                outputCostPerMillion = 5m;
                costPer1000Requests = 5m; // Using the low tier pricing
                break;
            case "sonar-reasoning-pro":
                inputCostPerMillion = 2m;
                outputCostPerMillion = 8m;
                costPer1000Requests = 6m; // Using the low tier pricing
                break;
            case "sonar-deep-research":
                inputCostPerMillion = 2m;
                outputCostPerMillion = 8m;
                inferenceCostPerMillion = 3m;
                costPer1000Requests = 5m;
                break;
            case "r1-1776": // Offline model
                inputCostPerMillion = 2m;
                outputCostPerMillion = 8m;
                costPer1000Requests = 0m; // No search queries for offline model
                break;
            default:
                // Default to sonar pricing if model not recognized
                inputCostPerMillion = 1m;
                outputCostPerMillion = 1m;
                costPer1000Requests = 5m;
                break;
        }

        // Calculate costs
        decimal inputCost = (inputTokens / 1000000m) * inputCostPerMillion;
        decimal outputCost = (outputTokens / 1000000m) * outputCostPerMillion;

        // Calculate inference cost (only for deep research model)
        decimal inferenceCost = 0;
        if (modelId == "sonar-deep-research")
        {
            // Assuming inference tokens are roughly half of input tokens for estimation
            int inferenceTokens = inputTokens / 2;
            inferenceCost = (inferenceTokens / 1000000m) * inferenceCostPerMillion;
        }

        // Calculate request cost (per execution)
        decimal requestCost = (searchQueries / 1000m) * costPer1000Requests;

        // Total cost
        decimal totalCost = inputCost + outputCost + inferenceCost + requestCost;

        return totalCost;
    }

    // Calculate OpenAI web search cost for a single execution
    protected static decimal CalculateOpenAIWebSearchCost(string AImodel, int searchContextSize = 2, int estimatedInternalSearches = 1)
    {
        decimal costPer1000Calls = 0;

        // Determine base cost per 1000 calls based on model and context size
        // 1 = low, 2 = medium (default), 3 = high
        if (AImodel == "gpt-4o-search-preview")
        {
            switch (searchContextSize)
            {
                case 1: // low
                    costPer1000Calls = 30.00m;
                    break;
                case 2: // medium (default)
                    costPer1000Calls = 35.00m;
                    break;
                case 3: // high
                    costPer1000Calls = 50.00m;
                    break;
                default:
                    costPer1000Calls = 35.00m; // Default to medium
                    break;
            }
        }
        else if (AImodel == "gpt-4o-mini-search-preview")
        {
            switch (searchContextSize)
            {
                case 1: // low
                    costPer1000Calls = 25.00m;
                    break;
                case 2: // medium (default)
                    costPer1000Calls = 27.50m;
                    break;
                case 3: // high
                    costPer1000Calls = 30.00m;
                    break;
                default:
                    costPer1000Calls = 27.50m; // Default to medium
                    break;
            }
        }
        else
        {
            // Default to medium context GPT-4o pricing if model not recognized
            costPer1000Calls = 35.00m;
        }

        // Calculate cost per call
        decimal costPerCall = costPer1000Calls / 1000m;

        // Calculate total cost including estimated internal searches
        decimal totalCost = costPerCall * estimatedInternalSearches;

        return totalCost;
    }

}
