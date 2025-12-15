namespace ChatbotService.Models;

/// <summary>
/// Represents chat history for a session
/// </summary>
public class ChatHistoryResponse
{
    /// <summary>
    /// The session identifier
    /// </summary>
    public required string SessionId { get; set; }

    /// <summary>
    /// List of messages in the conversation
    /// </summary>
    public required List<ChatMessage> Messages { get; set; }

    /// <summary>
    /// Total number of messages in the session
    /// </summary>
    public int TotalCount { get; set; }
}

/// <summary>
/// Represents a single chat message
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Who sent the message (user or assistant)
    /// </summary>
    public required string Role { get; set; }

    /// <summary>
    /// The message content
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// When the message was sent
    /// </summary>
    public DateTime Timestamp { get; set; }
}
