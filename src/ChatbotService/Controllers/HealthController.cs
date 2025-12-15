using ChatbotService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatbotService.Controllers;

/// <summary>
/// Controller for health check operations
/// </summary>
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Health check endpoint
    /// </summary>
    /// <returns>Health status</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> GetHealth()
    {
        return Ok(new HealthResponse
        {
            Status = "healthy",
            Services = new Dictionary<string, string>
            {
                { "api", "healthy" }
            },
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Readiness check endpoint
    /// </summary>
    /// <returns>Readiness status</returns>
    [HttpGet("ready")]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status503ServiceUnavailable)]
    public ActionResult<HealthResponse> GetReadiness()
    {
        // In a real application, you would check if all dependencies are ready
        // For now, we'll just return healthy
        return Ok(new HealthResponse
        {
            Status = "healthy",
            Services = new Dictionary<string, string>
            {
                { "api", "healthy" },
                { "azureOpenAI", "healthy" }
            },
            Timestamp = DateTime.UtcNow
        });
    }
}
