public abstract class BaseChatService
{
    // Dictionary to store conversation histories for different model IDs
    protected static Dictionary<string, List<object>> _conversationHistories = new();

    // Dictionary to store system messages for different model IDs
    protected static Dictionary<string, object> _systemMessages = new();

    // Thread-safe locks for dictionary operations
    protected static readonly object _conversationLock = new object();
    protected static readonly object _systemMessageLock = new object();

    // Default memory size
    protected static int _memorySize = 10;

    // Method to initialize conversation for a model if it doesn't exist
    protected static void EnsureConversationExists(string modelId)
    {
        lock (_conversationLock)
        {
            if (!_conversationHistories.ContainsKey(modelId))
            {
                _conversationHistories[modelId] = new List<object>();
            }
        }
    }

    // Add a permanent system message for a specific model
    public static void AddPermanentSystemMessage(string modelId, string message)
    {
        EnsureConversationExists(modelId);
        lock (_systemMessageLock)
        {
            _systemMessages[modelId] = new { role = "system", content = message };
        }
    }

    // Set memory size for conversation history
    public static void SetMemorySize(int size)
    {
        if (size > 0)
        {
            _memorySize = size;
            // Trim all existing conversations
            lock (_conversationLock)
            {
                foreach (var modelId in _conversationHistories.Keys.ToList())
                {
                    TrimConversationHistoryInternal(modelId);
                }
            }
        }
    }

    // Trim conversation history to memory size
    protected static void TrimConversationHistory(string modelId)
    {
        lock (_conversationLock)
        {
            TrimConversationHistoryInternal(modelId);
        }
    }

    // Internal method for trimming (assumes lock is already held)
    private static void TrimConversationHistoryInternal(string modelId)
    {
        EnsureConversationExistsInternal(modelId);

        if (_conversationHistories[modelId].Count > _memorySize * 2) // Multiply by 2 because each exchange has user + assistant messages
        {
            _conversationHistories[modelId] = _conversationHistories[modelId]
                .Skip(_conversationHistories[modelId].Count - _memorySize * 2)
                .ToList();
        }
    }

    // Internal method for ensuring conversation exists (assumes lock is already held)
    private static void EnsureConversationExistsInternal(string modelId)
    {
        if (!_conversationHistories.ContainsKey(modelId))
        {
            _conversationHistories[modelId] = new List<object>();
        }
    }

    // Clear conversation history for a specific model
    public static void ClearConversationHistory(string modelId)
    {
        lock (_conversationLock)
        {
            EnsureConversationExistsInternal(modelId);
            _conversationHistories[modelId].Clear();
        }
    }

    // Clear all conversation histories
    public static void ClearAllConversationHistories()
    {
        lock (_conversationLock)
        {
            try
            {
                foreach (var modelId in _conversationHistories.Keys.ToList())
                {
                    try
                    {
                        _conversationHistories[modelId].Clear();
                    }
                    catch
                    {
                        // Ignore any errors and continue without clearing
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log or handle the exception as needed
                System.Diagnostics.Debug.WriteLine($"Error clearing conversation histories: {ex.Message}");
            }
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

    // Thread-safe helper methods for derived classes to access conversation histories
    protected static List<object> GetConversationHistorySafe(string modelId)
    {
        lock (_conversationLock)
        {
            EnsureConversationExistsInternal(modelId);
            return new List<object>(_conversationHistories[modelId]); // Return a copy to avoid external modification
        }
    }

    protected static void AddToConversationHistorySafe(string modelId, object message)
    {
        lock (_conversationLock)
        {
            EnsureConversationExistsInternal(modelId);
            _conversationHistories[modelId].Add(message);
        }
    }

    protected static bool ContainsSystemMessageSafe(string modelId)
    {
        lock (_systemMessageLock)
        {
            return _systemMessages.ContainsKey(modelId);
        }
    }

    protected static object GetSystemMessageSafe(string modelId)
    {
        lock (_systemMessageLock)
        {
            return _systemMessages.TryGetValue(modelId, out var message) ? message : null;
        }
    }

}
