namespace ChatbotService.Configuration;

/// <summary>
/// Configuration options for the chat service
/// </summary>
public class ChatServiceOptions
{
    public const string SectionName = "ChatService";

    /// <summary>
    /// Maximum number of history messages to keep per session
    /// </summary>
    public int MaxHistoryMessages { get; set; } = 50;

    /// <summary>
    /// Session timeout in minutes
    /// </summary>
    public int SessionTimeoutMinutes { get; set; } = 30;
}
