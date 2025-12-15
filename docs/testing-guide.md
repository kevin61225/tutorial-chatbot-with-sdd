# Testing Guide

This document provides comprehensive testing instructions for the Chatbot Service.

## Prerequisites

- .NET 10 SDK installed
- cURL or Postman for API testing
- (Optional) Docker for container testing

## Building the Project

### Using .NET CLI

```bash
# Restore dependencies
dotnet restore ChatbotService.sln

# Build the solution
dotnet build ChatbotService.sln

# Build in Release mode
dotnet build ChatbotService.sln --configuration Release
```

### Using the Quick Start Script

```bash
# Make the script executable (Linux/macOS)
chmod +x start.sh

# Run the script
./start.sh
```

## Running the Application

### Local Development

```bash
# Navigate to the project directory
cd src/ChatbotService

# Run the application
dotnet run
```

The API will be available at `http://localhost:5000` (or the port specified in launchSettings.json).

### With Custom Port

```bash
cd src/ChatbotService
dotnet run --urls "http://localhost:8080"
```

## API Testing

### 1. Health Check

Test if the service is running:

```bash
curl http://localhost:5000/health
```

Expected response:
```json
{
  "status": "healthy",
  "services": {
    "api": "healthy"
  },
  "timestamp": "2025-12-15T10:30:00Z"
}
```

### 2. Readiness Check

Test if the service is ready to accept requests:

```bash
curl http://localhost:5000/health/ready
```

Expected response:
```json
{
  "status": "healthy",
  "services": {
    "api": "healthy",
    "azureOpenAI": "healthy"
  },
  "timestamp": "2025-12-15T10:30:00Z"
}
```

### 3. Send Chat Message

**Note**: This requires valid Azure OpenAI credentials configured in appsettings.json or environment variables.

```bash
curl -X POST http://localhost:5000/api/chat/message \
  -H "Content-Type: application/json" \
  -d '{
    "message": "Hello, how can I reset my password?",
    "sessionId": "test-session-001",
    "context": {
      "userId": "user-123",
      "clientType": "web"
    }
  }'
```

Expected response:
```json
{
  "message": "To reset your password...",
  "sessionId": "test-session-001",
  "timestamp": "2025-12-15T10:30:00Z",
  "metadata": {
    "tokensUsed": 150
  }
}
```

### 4. Get Chat History

```bash
curl http://localhost:5000/api/chat/sessions/test-session-001/history?limit=10
```

Expected response:
```json
{
  "sessionId": "test-session-001",
  "messages": [
    {
      "role": "user",
      "content": "Hello, how can I reset my password?",
      "timestamp": "2025-12-15T10:30:00Z"
    },
    {
      "role": "assistant",
      "content": "To reset your password...",
      "timestamp": "2025-12-15T10:30:01Z"
    }
  ],
  "totalCount": 2
}
```

## Testing with Postman

1. Import the OpenAPI specification: `specs/openapi.yaml`
2. Postman will automatically create a collection with all endpoints
3. Update the base URL to match your running instance
4. Test each endpoint

## Docker Testing

### Build the Docker Image

```bash
docker build -t chatbot-service:latest .
```

### Run with Docker

```bash
docker run -d \
  -p 8080:8080 \
  -e AzureOpenAI__Endpoint="https://your-resource.openai.azure.com/" \
  -e AzureOpenAI__ApiKey="your-api-key" \
  -e AzureOpenAI__DeploymentName="your-deployment-name" \
  --name chatbot-service \
  chatbot-service:latest
```

### Test Docker Container

```bash
# Health check
curl http://localhost:8080/health

# View logs
docker logs chatbot-service

# Stop container
docker stop chatbot-service

# Remove container
docker rm chatbot-service
```

### Run with Docker Compose

```bash
# Start services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

## Configuration for Testing

### Using appsettings.Development.json

Create a file `src/ChatbotService/appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AzureOpenAI": {
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key-here",
    "DeploymentName": "your-deployment-name",
    "MaxTokens": 1000,
    "Temperature": 0.7
  }
}
```

### Using Environment Variables

```bash
export AzureOpenAI__Endpoint="https://your-resource.openai.azure.com/"
export AzureOpenAI__ApiKey="your-api-key"
export AzureOpenAI__DeploymentName="your-deployment-name"

dotnet run
```

## Error Testing

### Test Invalid Request (Missing Message)

```bash
curl -X POST http://localhost:5000/api/chat/message \
  -H "Content-Type: application/json" \
  -d '{
    "sessionId": "test-session-001"
  }'
```

Expected response (400 Bad Request):
```json
{
  "error": "InvalidRequest",
  "message": "Message cannot be empty",
  "timestamp": "2025-12-15T10:30:00Z"
}
```

### Test Invalid Request (Missing SessionId)

```bash
curl -X POST http://localhost:5000/api/chat/message \
  -H "Content-Type: application/json" \
  -d '{
    "message": "Hello"
  }'
```

Expected response (400 Bad Request):
```json
{
  "error": "InvalidRequest",
  "message": "SessionId is required",
  "timestamp": "2025-12-15T10:30:00Z"
}
```

### Test Non-Existent Session History

```bash
curl http://localhost:5000/api/chat/sessions/non-existent-session/history
```

Expected response (404 Not Found):
```json
{
  "error": "NotFound",
  "message": "Session non-existent-session not found",
  "timestamp": "2025-12-15T10:30:00Z"
}
```

## Performance Testing

### Using Apache Bench (ab)

```bash
# Install Apache Bench if needed
# Ubuntu/Debian: sudo apt-get install apache2-utils
# macOS: Already included

# Test health endpoint (100 requests, 10 concurrent)
ab -n 100 -c 10 http://localhost:5000/health
```

### Using wrk

```bash
# Install wrk
# Ubuntu/Debian: sudo apt-get install wrk
# macOS: brew install wrk

# Test health endpoint for 30 seconds
wrk -t4 -c100 -d30s http://localhost:5000/health
```

## Troubleshooting

### Application Won't Start

1. Check if the port is already in use:
   ```bash
   lsof -i :5000  # Linux/macOS
   netstat -ano | findstr :5000  # Windows
   ```

2. Check .NET version:
   ```bash
   dotnet --version
   ```
   Should be 10.0.x or higher

### Docker Build Fails

1. Ensure Docker is running:
   ```bash
   docker --version
   ```

2. Check Docker daemon status:
   ```bash
   docker ps
   ```

3. Try building with verbose output:
   ```bash
   docker build --progress=plain -t chatbot-service:latest .
   ```

### Azure OpenAI Connection Issues

1. Verify endpoint URL is correct
2. Check API key is valid
3. Ensure deployment name matches your Azure OpenAI resource
4. Check network connectivity to Azure

## Next Steps

After successful testing:

1. Add unit tests for services
2. Add integration tests for controllers
3. Set up CI/CD pipeline
4. Configure monitoring and logging
5. Deploy to production environment

## Additional Resources

- [.NET Testing Documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/)
- [Docker Documentation](https://docs.docker.com/)
- [Azure OpenAI Documentation](https://learn.microsoft.com/en-us/azure/ai-services/openai/)
