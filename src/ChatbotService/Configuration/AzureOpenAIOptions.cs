namespace ChatbotService.Configuration;

/// <summary>
/// Configuration options for Azure OpenAI
/// </summary>
public class AzureOpenAIOptions
{
    public const string SectionName = "AzureOpenAI";

    /// <summary>
    /// Azure OpenAI endpoint URL
    /// </summary>
    public required string Endpoint { get; set; }

    /// <summary>
    /// API key for authentication
    /// </summary>
    public required string ApiKey { get; set; }

    /// <summary>
    /// Deployment name for the model
    /// </summary>
    public required string DeploymentName { get; set; }

    /// <summary>
    /// Maximum tokens for completion
    /// </summary>
    public int MaxTokens { get; set; } = 1000;

    /// <summary>
    /// Temperature for response generation (0.0 - 1.0)
    /// </summary>
    public float Temperature { get; set; } = 0.7f;
}
