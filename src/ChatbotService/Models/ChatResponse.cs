namespace ChatbotService.Models;

/// <summary>
/// Represents a chat response from the chatbot
/// </summary>
public class ChatResponse
{
    /// <summary>
    /// The chatbot's response message
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// The session identifier
    /// </summary>
    public required string SessionId { get; set; }

    /// <summary>
    /// When the response was generated
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Additional response metadata
    /// </summary>
    public ResponseMetadata? Metadata { get; set; }
}

/// <summary>
/// Metadata for chat response
/// </summary>
public class ResponseMetadata
{
    /// <summary>
    /// Number of tokens consumed
    /// </summary>
    public int? TokensUsed { get; set; }

    /// <summary>
    /// Confidence score of the response
    /// </summary>
    public float? Confidence { get; set; }
}
