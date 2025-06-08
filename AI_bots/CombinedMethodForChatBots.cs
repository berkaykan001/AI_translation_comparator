public class All_AI_Chat_Bots
{
    public static async Task<string> AskAI(string LLM, string AImodel, string systemRole, string userMessage)
    {
        var result = await AskAIExtended(LLM, AImodel, systemRole, userMessage);
        return $"{result.text} (Answer cost: ${result.estimatedCost})";
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskAIExtended(
        string LLM, string AImodel, string systemRole, string userMessage)
    {
        switch (LLM)
        {
            case "OpenAI":
                return await OpenAIChat.AskOpenai(AImodel, systemRole, userMessage);
            case "MistralAI":
                return await MistralChat.AskMistral(AImodel, systemRole, userMessage);
            case "DeepSeek":
                return await DeepSeekChat.AskDeepSeek(AImodel, systemRole, userMessage);
            case "Claude":
                return await ClaudeChat.AskClaude(AImodel, systemRole, userMessage);
            case "ClaudeWeb":
                return await ClaudeChat.AskClaudeWebSearch(AImodel, systemRole, userMessage);
            case "Perplexity":
                return await PerplexityChat.AskPerplexity(AImodel, systemRole, userMessage);
            case "Gemini":
                return await GeminiChat.AskGemini(AImodel, systemRole, userMessage);
            case "LLMapi":
                return await LLMapiChat.AskLLMapi(AImodel, systemRole, userMessage);
            case "Grok":
                return await GrokChat.AskGrok(AImodel, systemRole, userMessage);
            case "GrokWeb":
                return await GrokChat.GrokWebSearch(AImodel, systemRole, userMessage);
            case "OpenRouter":
                return await OpenRouterChat.AskOpenRouter(AImodel, systemRole, userMessage);
            default:
                return ("Unknown LLM. Choose OpenAI, MistralAI, DeepSeek, Claude, Perplexity, Gemini, Grok or LLMapi", 0, 0);
        }
    }

    public static async Task<string> AskFollowUp(string LLM, string AImodel, string systemRole, string followUpQuestion)
    {
        var result = await AskFollowUpExtended(LLM, AImodel, systemRole, followUpQuestion);
        return $"{result.text} (Answer cost: ${result.estimatedCost})";
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskFollowUpExtended(
        string LLM, string AImodel, string systemRole, string followUpQuestion)
    {
        switch (LLM)
        {
            case "OpenAI":
                return await OpenAIChat.AskOpenaiFollowUp(AImodel, systemRole, followUpQuestion);
            case "Claude":
                return await ClaudeChat.AskClaudeFollowUp(AImodel, systemRole, followUpQuestion);
            case "ClaudeWeb":
                return await ClaudeChat.AskClaudeWebSearchFollowUp(AImodel, systemRole, followUpQuestion);
            case "MistralAI":
                return await MistralChat.AskMistralFollowUp(AImodel, systemRole, followUpQuestion);
            case "Perplexity":
                return await PerplexityChat.AskPerplexityFollowUp(AImodel, systemRole, followUpQuestion);
            case "Gemini":
                return await GeminiChat.AskGeminiFollowUp(AImodel, systemRole, followUpQuestion);
            case "DeepSeek":
                return await DeepSeekChat.AskDeepSeekFollowUp(AImodel, systemRole, followUpQuestion);
            case "LLMapi":
                return await LLMapiChat.AskLLMapiFollowUp(AImodel, systemRole, followUpQuestion);
            case "Grok":
                return await GrokChat.AskGrokFollowUp(AImodel, systemRole, followUpQuestion);
            case "GrokWeb":
                return await GrokChat.GrokWebSearchFollowUp(AImodel, systemRole, followUpQuestion);
            case "OpenRouter":
                return await OpenRouterChat.AskOpenRouterFollowUp(AImodel, systemRole, followUpQuestion);
            default:
                return ("Follow-up not yet implemented for this AI provider.", 0, 0);
        }
    }

    // New method for image generation
    public static async Task<(string imageUrl, long elapsedMs, double estimatedCost)> GenerateImage(
        string LLM, string AImodel, string prompt)
    {
        switch (LLM)
        {
            case "OpenAI":
                return await OpenAIChat.OpenaiGenerateImage(AImodel, prompt);
            case "Gemini":
                return await GeminiChat.GeminiGenerateImage(AImodel, prompt);
            case "Grok":
                return await GrokChat.GrokGenerateImage(AImodel, prompt);
            default:
                return ("Image generation not supported for this LLM provider.", 0, 0);
        }
    }

    public static void ClearConversation(string LLM, string AImodel)
    {
        switch (LLM)
        {
            case "OpenAI":
                OpenAIChat.ClearConversationHistory(AImodel);
                break;
            case "Claude":
                ClaudeChat.ClearConversationHistory(AImodel);
                break;
            case "MistralAI":
                MistralChat.ClearConversationHistory(AImodel);
                break;
            case "Perplexity":
                PerplexityChat.ClearConversationHistory(AImodel);
                break;
            case "Gemini":
                GeminiChat.ClearConversationHistory(AImodel);
                break;
            case "DeepSeek":
                DeepSeekChat.ClearConversationHistory(AImodel);
                break;
            case "LLMapi":
                LLMapiChat.ClearConversationHistory(AImodel);
                break;
            case "Grok":
                GrokChat.ClearConversationHistory(AImodel);
                break;
            case "OpenRouter":
                OpenRouterChat.ClearConversationHistory(AImodel);
                break;
            default:
                break;
        }
    }

    public static void ClearAllConversations()
    {
        BaseChatService.ClearAllConversationHistories();
    }
}
