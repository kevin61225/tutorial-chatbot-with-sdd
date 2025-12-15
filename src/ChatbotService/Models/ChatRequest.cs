namespace ChatbotService.Models;

/// <summary>
/// Represents a chat request from the client
/// </summary>
public class ChatRequest
{
    /// <summary>
    /// The user's message to the chatbot
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Unique identifier for the chat session
    /// </summary>
    public required string SessionId { get; set; }

    /// <summary>
    /// Additional context information
    /// </summary>
    public ChatContext? Context { get; set; }
}

/// <summary>
/// Context information for a chat request
/// </summary>
public class ChatContext
{
    /// <summary>
    /// User identifier
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Type of client application (web, teams, mobile)
    /// </summary>
    public string? ClientType { get; set; }

    /// <summary>
    /// Additional metadata
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }
}
