using ChatbotService.Models;

namespace ChatbotService.Services;

/// <summary>
/// Interface for chatbot service operations
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Send a message and get a response from the chatbot
    /// </summary>
    Task<ChatResponse> SendMessageAsync(ChatRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get chat history for a session
    /// </summary>
    Task<ChatHistoryResponse> GetChatHistoryAsync(string sessionId, int limit = 50, CancellationToken cancellationToken = default);
}
