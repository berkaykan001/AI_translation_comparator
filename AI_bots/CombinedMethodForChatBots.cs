public class All_AI_Chat_Bots
{
    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskAI(
        string LLM, string AImodel, string systemRole, string userMessage)
    {
        switch (LLM)
        {
            case "OpenAI":
                return await OpenAIChat.AskOpenai(AImodel, systemRole, userMessage);
            case "Claude":
                return await ClaudeChat.AskClaude(AImodel, systemRole, userMessage);
            case "LLMapi":
                return await LLMapiChat.AskLLMapi(AImodel, systemRole, userMessage);
            case "OpenRouter":
                return await OpenRouterChat.AskOpenRouter(AImodel, systemRole, userMessage);
            case "DeepSeek":
                return await DeepSeekChat.AskDeepSeek(AImodel, systemRole, userMessage);
            case "Gemini":
                return await GeminiChat.AskGemini(AImodel, systemRole, userMessage);
            case "Grok":
                return await GrokChat.AskGrok(AImodel, systemRole, userMessage);
            case "Mistral":
                return await MistralChat.AskMistral(AImodel, systemRole, userMessage);
            case "Perplexity":
                return await PerplexityChat.AskPerplexity(AImodel, systemRole, userMessage);
            default:
                return ("Unknown LLM. Choose OpenAI, Claude, LLMapi, OpenRouter, DeepSeek, Gemini, Grok, Mistral, or Perplexity", 0, 0);
        }
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskFollowUp(
        string LLM, string AImodel, string systemRole, string followUpQuestion)
    {
        switch (LLM)
        {
            case "OpenAI":
                return await OpenAIChat.AskOpenaiFollowUp(AImodel, systemRole, followUpQuestion);
            case "Claude":
                return await ClaudeChat.AskClaudeFollowUp(AImodel, systemRole, followUpQuestion);
            case "LLMapi":
                return await LLMapiChat.AskLLMapiFollowUp(AImodel, systemRole, followUpQuestion);
            case "OpenRouter":
                return await OpenRouterChat.AskOpenRouterFollowUp(AImodel, systemRole, followUpQuestion);
            case "DeepSeek":
                return await DeepSeekChat.AskDeepSeekFollowUp(AImodel, systemRole, followUpQuestion);
            case "Gemini":
                return await GeminiChat.AskGeminiFollowUp(AImodel, systemRole, followUpQuestion);
            case "Grok":
                return await GrokChat.AskGrokFollowUp(AImodel, systemRole, followUpQuestion);
            case "Mistral":
                return await MistralChat.AskMistralFollowUp(AImodel, systemRole, followUpQuestion);
            case "Perplexity":
                return await PerplexityChat.AskPerplexityFollowUp(AImodel, systemRole, followUpQuestion);
            default:
                return ("Follow-up not yet implemented for this AI provider.", 0, 0);
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
            case "LLMapi":
                LLMapiChat.ClearConversationHistory(AImodel);
                break;
            case "OpenRouter":
                OpenRouterChat.ClearConversationHistory(AImodel);
                break;
            case "DeepSeek":
                DeepSeekChat.ClearConversationHistory(AImodel);
                break;
            case "Gemini":
                GeminiChat.ClearConversationHistory(AImodel);
                break;
            case "Grok":
                GrokChat.ClearConversationHistory(AImodel);
                break;
            case "Mistral":
                MistralChat.ClearConversationHistory(AImodel);
                break;
            case "Perplexity":
                PerplexityChat.ClearConversationHistory(AImodel);
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
