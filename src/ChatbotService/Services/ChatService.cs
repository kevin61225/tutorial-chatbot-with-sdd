using Azure;
using Azure.AI.OpenAI;
using ChatbotService.Configuration;
using ChatbotService.Models;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using ChatMessageModel = ChatbotService.Models.ChatMessage;

namespace ChatbotService.Services;

/// <summary>
/// Implementation of chat service using Azure OpenAI
/// </summary>
public class ChatService : IChatService
{
    private readonly AzureOpenAIClient _openAIClient;
    private readonly ChatClient _chatClient;
    private readonly AzureOpenAIOptions _openAIOptions;
    private readonly ChatServiceOptions _chatOptions;
    private readonly ILogger<ChatService> _logger;
    
    // In-memory storage for chat sessions (in production, use a database)
    private readonly Dictionary<string, List<ChatMessageModel>> _sessions = new();
    private readonly object _sessionsLock = new();

    public ChatService(
        IOptions<AzureOpenAIOptions> openAIOptions,
        IOptions<ChatServiceOptions> chatOptions,
        ILogger<ChatService> logger)
    {
        _openAIOptions = openAIOptions.Value;
        _chatOptions = chatOptions.Value;
        _logger = logger;

        // Initialize Azure OpenAI client
        _openAIClient = new AzureOpenAIClient(
            new Uri(_openAIOptions.Endpoint),
            new AzureKeyCredential(_openAIOptions.ApiKey));

        _chatClient = _openAIClient.GetChatClient(_openAIOptions.DeploymentName);
    }

    public async Task<ChatResponse> SendMessageAsync(ChatRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing chat message for session {SessionId}", request.SessionId);

        try
        {
            // Get conversation history
            var history = GetSessionHistory(request.SessionId);

            // Add user message to history
            var userMessage = new ChatMessageModel
            {
                Role = "user",
                Content = request.Message,
                Timestamp = DateTime.UtcNow
            };
            AddMessageToSession(request.SessionId, userMessage);

            // Build messages for Azure OpenAI
            var messages = new List<OpenAI.Chat.ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant that answers questions about a SaaS application. " +
                    "Provide clear, accurate, and helpful responses based on the documentation you have been trained on.")
            };

            // Add conversation history
            foreach (var msg in history.TakeLast(10)) // Limit context window
            {
                if (msg.Role == "user")
                    messages.Add(new UserChatMessage(msg.Content));
                else if (msg.Role == "assistant")
                    messages.Add(new AssistantChatMessage(msg.Content));
            }

            // Get completion from Azure OpenAI
            var chatCompletionOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = _openAIOptions.MaxTokens,
                Temperature = _openAIOptions.Temperature
            };

            var completion = await _chatClient.CompleteChatAsync(messages, chatCompletionOptions, cancellationToken);

            var responseContent = completion.Value.Content[0].Text;
            var tokensUsed = completion.Value.Usage.TotalTokenCount;

            // Add assistant message to history
            var assistantMessage = new ChatMessageModel
            {
                Role = "assistant",
                Content = responseContent,
                Timestamp = DateTime.UtcNow
            };
            AddMessageToSession(request.SessionId, assistantMessage);

            return new ChatResponse
            {
                Message = responseContent,
                SessionId = request.SessionId,
                Timestamp = DateTime.UtcNow,
                Metadata = new ResponseMetadata
                {
                    TokensUsed = tokensUsed
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat message for session {SessionId}", request.SessionId);
            throw;
        }
    }

    public Task<ChatHistoryResponse> GetChatHistoryAsync(string sessionId, int limit = 50, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving chat history for session {SessionId}", sessionId);

        var history = GetSessionHistory(sessionId);
        var messages = history.TakeLast(limit).ToList();

        return Task.FromResult(new ChatHistoryResponse
        {
            SessionId = sessionId,
            Messages = messages,
            TotalCount = history.Count
        });
    }

    private List<ChatMessageModel> GetSessionHistory(string sessionId)
    {
        lock (_sessionsLock)
        {
            if (!_sessions.ContainsKey(sessionId))
            {
                _sessions[sessionId] = new List<ChatMessageModel>();
            }
            return _sessions[sessionId];
        }
    }

    private void AddMessageToSession(string sessionId, ChatMessageModel message)
    {
        lock (_sessionsLock)
        {
            var history = GetSessionHistory(sessionId);
            history.Add(message);

            // Limit history size
            if (history.Count > _chatOptions.MaxHistoryMessages)
            {
                history.RemoveAt(0);
            }
        }
    }
}
