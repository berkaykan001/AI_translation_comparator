public class All_AI_Chat_Bots
{
    public static async Task<string> AskAI(string LLM, string AImodel, string systemRole, string userMessage)
    {
        var result = await AskAIExtended(LLM, AImodel, systemRole, userMessage);
        return result.text;
    }

    public static async Task<(string text, long elapsedMs, decimal estimatedCost)> AskAIExtended(
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
            default:
                return ("Unknown LLM. Choose OpenAI, Claude, LLMapi or OpenRouter", 0, 0);
        }
    }

    public static async Task<string> AskFollowUp(string LLM, string AImodel, string systemRole, string followUpQuestion)
    {
        var result = await AskFollowUpExtended(LLM, AImodel, systemRole, followUpQuestion);
        return result.text;
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
            case "LLMapi":
                return await LLMapiChat.AskLLMapiFollowUp(AImodel, systemRole, followUpQuestion);
            case "OpenRouter":
                return await OpenRouterChat.AskOpenRouterFollowUp(AImodel, systemRole, followUpQuestion);
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
            default:
                break;
        }
    }

    public static void ClearAllConversations()
    {
        BaseChatService.ClearAllConversationHistories();
    }
}
