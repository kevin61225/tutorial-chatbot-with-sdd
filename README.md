# Tutorial: Chatbot with Spec-Driven Design

A chatbot service built using Spec-Driven Design (SDD) methodology with .NET 10 and Docker. This service provides intelligent responses using Azure OpenAI and supports multiple client applications including web applications and Microsoft Teams apps.

## 🎯 Project Overview

This project demonstrates how to build a production-ready chatbot service following Spec-Driven Design principles:

- **Specification-First Development**: All features begin with detailed API specifications (OpenAPI)
- **Contract-Driven**: API contracts are defined before implementation
- **Documentation as Code**: Specifications serve as both design documents and API documentation

## 🏗️ Architecture

```
┌─────────────────┐         ┌──────────────────┐
│  Web Client     │         │  Teams Client    │
└────────┬────────┘         └────────┬─────────┘
         │                           │
         └───────────┬───────────────┘
                     │
                     ▼
         ┌───────────────────────┐
         │   Chatbot Service     │
         │   (.NET 10 API)       │
         └───────────┬───────────┘
                     │
                     ▼
         ┌───────────────────────┐
         │   Azure OpenAI        │
         └───────────────────────┘
```

## 🚀 Technology Stack

- **.NET 10**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API framework
- **Azure OpenAI**: AI-powered chat completions
- **Docker**: Containerization
- **OpenAPI 3.0**: API specification and documentation

## 📋 Features

- **Chat Endpoints**:
  - Send messages to the chatbot
  - Retrieve chat history by session
  
- **Health Monitoring**:
  - Health check endpoint
  - Readiness probe for orchestration

- **Multiple Client Support**:
  - Web applications
  - Microsoft Teams apps
  - Mobile apps (future)

## 🔧 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- Azure OpenAI resource (for production use)

## 📦 Project Structure

```
.
├── docs/
│   └── architecture.md          # Architecture documentation
├── specs/
│   └── openapi.yaml            # OpenAPI specification
├── src/
│   └── ChatbotService/
│       ├── Configuration/      # Configuration models
│       ├── Controllers/        # API controllers
│       ├── Models/            # Data models
│       ├── Services/          # Business logic
│       └── Program.cs         # Application entry point
├── Dockerfile                 # Docker image definition
├── docker-compose.yml        # Docker Compose configuration
└── ChatbotService.sln       # Solution file
```

## 🛠️ Getting Started

### Local Development

1. **Clone the repository**:
   ```bash
   git clone https://github.com/kevin61225/tutorial-chatbot-with-sdd.git
   cd tutorial-chatbot-with-sdd
   ```

2. **Configure Azure OpenAI** (Optional for development):
   
   Update `src/ChatbotService/appsettings.Development.json`:
   ```json
   {
     "AzureOpenAI": {
       "Endpoint": "https://your-resource.openai.azure.com/",
       "ApiKey": "your-api-key",
       "DeploymentName": "your-deployment-name"
     }
   }
   ```

3. **Build and run with .NET CLI**:
   ```bash
   cd src/ChatbotService
   dotnet restore
   dotnet build
   dotnet run
   ```

   The API will be available at `http://localhost:5008` (configured in `src/ChatbotService/Properties/launchSettings.json`)

### Docker Deployment

1. **Build the Docker image**:
   ```bash
   docker build -t chatbot-service .
   ```

2. **Run with Docker Compose**:
   ```bash
   docker-compose up -d
   ```

   The API will be available at `http://localhost:8080`

3. **Configure environment variables**:
   
   Create a `.env` file:
   ```env
   AZURE_OPENAI_ENDPOINT=https://your-resource.openai.azure.com/
   AZURE_OPENAI_API_KEY=your-api-key
   AZURE_OPENAI_DEPLOYMENT=your-deployment-name
   ```

## 📝 API Documentation

### Endpoints

#### Send Message
```http
POST /api/chat/message
Content-Type: application/json

{
  "message": "How do I reset my password?",
  "sessionId": "user-123-session-456",
  "context": {
    "userId": "user-123",
    "clientType": "web"
  }
}
```

#### Get Chat History
```http
GET /api/chat/sessions/{sessionId}/history?limit=50
```

#### Health Check
```http
GET /health
```

#### Readiness Check
```http
GET /health/ready
```

For detailed API documentation, see [specs/openapi.yaml](specs/openapi.yaml).

## 🧪 Testing

### Test the API with curl

```bash
# Health check
curl http://localhost:8080/health

# Send a message
curl -X POST http://localhost:8080/api/chat/message \
  -H "Content-Type: application/json" \
  -d '{
    "message": "Hello, chatbot!",
    "sessionId": "test-session-001"
  }'

# Get chat history
curl http://localhost:8080/api/chat/sessions/test-session-001/history
```

## 🔐 Configuration

### Application Settings

The service can be configured via `appsettings.json` or environment variables:

| Setting | Environment Variable | Description |
|---------|---------------------|-------------|
| `AzureOpenAI:Endpoint` | `AzureOpenAI__Endpoint` | Azure OpenAI endpoint URL |
| `AzureOpenAI:ApiKey` | `AzureOpenAI__ApiKey` | API key for authentication |
| `AzureOpenAI:DeploymentName` | `AzureOpenAI__DeploymentName` | Model deployment name |
| `AzureOpenAI:MaxTokens` | `AzureOpenAI__MaxTokens` | Maximum tokens per response |
| `AzureOpenAI:Temperature` | `AzureOpenAI__Temperature` | Response temperature (0.0-1.0) |

## 📚 Spec-Driven Design Process

This project follows the SDD methodology:

1. **Define Specifications**: Start with OpenAPI spec ([specs/openapi.yaml](specs/openapi.yaml))
2. **Design Architecture**: Document system architecture ([docs/architecture.md](docs/architecture.md))
3. **Implement Features**: Build code that conforms to specifications
4. **Validate**: Ensure implementation matches specifications
5. **Iterate**: Refine specs and implementation together

## 🐳 Docker Details

The Docker image uses a multi-stage build:
- **Build stage**: Compiles the .NET application
- **Publish stage**: Creates optimized release build
- **Runtime stage**: Minimal ASP.NET Core runtime image

Security features:
- Runs as non-root user
- Minimal base image (aspnet:10.0)
- Health check configuration

## 📄 License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📖 Additional Resources

- [Architecture Documentation](docs/architecture.md)
- [API Specification](specs/openapi.yaml)
- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Azure OpenAI Documentation](https://learn.microsoft.com/en-us/azure/ai-services/openai/)
- [OpenAPI Specification](https://swagger.io/specification/)
 
