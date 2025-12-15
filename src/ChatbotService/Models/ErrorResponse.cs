namespace ChatbotService.Models;

/// <summary>
/// Represents an error response
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error type
    /// </summary>
    public required string Error { get; set; }

    /// <summary>
    /// Human-readable error message
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// Additional error details
    /// </summary>
    public Dictionary<string, object>? Details { get; set; }

    /// <summary>
    /// When the error occurred
    /// </summary>
    public DateTime Timestamp { get; set; }
}
