namespace ChatbotService.Models;

/// <summary>
/// Represents health check response
/// </summary>
public class HealthResponse
{
    /// <summary>
    /// Overall health status
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Health status of individual services
    /// </summary>
    public Dictionary<string, string>? Services { get; set; }

    /// <summary>
    /// Timestamp of the health check
    /// </summary>
    public DateTime Timestamp { get; set; }
}
