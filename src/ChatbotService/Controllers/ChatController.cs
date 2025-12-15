using ChatbotService.Models;
using ChatbotService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatbotService.Controllers;

/// <summary>
/// Controller for chat operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    /// <summary>
    /// Send a message to the chatbot
    /// </summary>
    /// <param name="request">The chat request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Chat response from the bot</returns>
    [HttpPost("message")]
    [ProducesResponseType(typeof(ChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ChatResponse>> SendMessage(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "InvalidRequest",
                    Message = "Message cannot be empty",
                    Timestamp = DateTime.UtcNow
                });
            }

            if (string.IsNullOrWhiteSpace(request.SessionId))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "InvalidRequest",
                    Message = "SessionId is required",
                    Timestamp = DateTime.UtcNow
                });
            }

            var response = await _chatService.SendMessageAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat message");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Error = "InternalError",
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Get chat history for a session
    /// </summary>
    /// <param name="sessionId">The session identifier</param>
    /// <param name="limit">Maximum number of messages to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Chat history</returns>
    [HttpGet("sessions/{sessionId}/history")]
    [ProducesResponseType(typeof(ChatHistoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChatHistoryResponse>> GetChatHistory(
        string sessionId,
        [FromQuery] int limit = 50,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (limit < 1 || limit > 100)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "InvalidRequest",
                    Message = "Limit must be between 1 and 100",
                    Timestamp = DateTime.UtcNow
                });
            }

            var history = await _chatService.GetChatHistoryAsync(sessionId, limit, cancellationToken);
            
            if (history.Messages.Count == 0)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "NotFound",
                    Message = $"Session {sessionId} not found",
                    Timestamp = DateTime.UtcNow
                });
            }

            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving chat history for session {SessionId}", sessionId);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
            {
                Error = "InternalError",
                Message = "An error occurred while retrieving chat history",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
