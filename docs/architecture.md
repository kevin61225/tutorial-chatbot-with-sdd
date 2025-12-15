# Chatbot Service Architecture

## Overview

This document describes the architecture of the Chatbot Service, built using Spec-Driven Design (SDD) methodology with .NET 10 and Docker.

## Spec-Driven Design Approach

This project follows the Spec-Driven Design methodology:

1. **Specification First**: All features begin with detailed specifications (OpenAPI, schemas)
2. **Contract-Driven**: API contracts are defined before implementation
3. **Validation**: Implementation is validated against specifications
4. **Documentation**: Specifications serve as both design and documentation

## System Architecture

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
         │   - Controllers       │
         │   - Services          │
         │   - Models            │
         └───────────┬───────────┘
                     │
                     ▼
         ┌───────────────────────┐
         │   Azure OpenAI        │
         │   - GPT Model         │
         │   - Document Index    │
         └───────────────────────┘
```

## Components

### 1. API Layer
- **Controllers**: Handle HTTP requests and responses
- **Models**: Define request/response schemas
- **Middleware**: Cross-cutting concerns (logging, error handling)

### 2. Service Layer
- **ChatService**: Manages chat interactions
- **AzureOpenAIService**: Integrates with Azure OpenAI
- **SessionService**: Manages chat sessions

### 3. Infrastructure Layer
- **Configuration**: Settings and secrets management
- **Logging**: Application logging
- **Health Checks**: Service health monitoring

## Technology Stack

- **Runtime**: .NET 10
- **Framework**: ASP.NET Core Web API
- **Container**: Docker
- **AI Service**: Azure OpenAI
- **API Documentation**: OpenAPI 3.0

## API Endpoints

### Chat Endpoints
- `POST /api/chat/message` - Send a message to the chatbot
- `GET /api/chat/sessions/{sessionId}/history` - Get chat history

### Health Endpoints
- `GET /health` - Health check
- `GET /health/ready` - Readiness check

## Configuration

The service requires the following configuration:

```json
{
  "AzureOpenAI": {
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key",
    "DeploymentName": "your-deployment-name",
    "MaxTokens": 1000,
    "Temperature": 0.7
  },
  "ChatService": {
    "MaxHistoryMessages": 50,
    "SessionTimeoutMinutes": 30
  }
}
```

## Docker Support

The application is containerized using Docker:
- Multi-stage build for optimized image size
- .NET 10 runtime base image
- Support for development and production environments

## Security Considerations

1. **API Key Management**: Azure OpenAI keys stored in secure configuration
2. **Input Validation**: All inputs validated against OpenAPI schema
3. **Rate Limiting**: Prevents abuse of the service
4. **Error Handling**: No sensitive data in error responses

## Deployment

The service can be deployed:
1. **Local Development**: Using docker-compose
2. **Container Registry**: Push to Azure Container Registry
3. **Azure Container Apps**: Production deployment
4. **Kubernetes**: For advanced orchestration needs

## Monitoring

- Health check endpoints for liveness and readiness probes
- Structured logging for observability
- Azure OpenAI integration status monitoring
