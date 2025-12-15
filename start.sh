#!/bin/bash

# Quick Start Script for Chatbot Service

set -e

echo "=========================================="
echo "Chatbot Service - Quick Start"
echo "=========================================="
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET 10 SDK is not installed."
    echo "Please install .NET 10 SDK from: https://dotnet.microsoft.com/download/dotnet/10.0"
    exit 1
fi

echo "✅ .NET version: $(dotnet --version)"
echo ""

# Build the solution
echo "📦 Building the solution..."
cd "$(dirname "$0")"
dotnet build ChatbotService.sln --configuration Release

echo ""
echo "✅ Build completed successfully!"
echo ""

# Ask if user wants to run the application
read -p "Do you want to run the application? (y/n) " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo ""
    echo "🚀 Starting the Chatbot Service..."
    echo "📍 API will be available at: http://localhost:5000"
    echo "📍 Health check: http://localhost:5000/health"
    echo "📍 API Docs: http://localhost:5000/swagger (in development mode)"
    echo ""
    echo "Press Ctrl+C to stop the service"
    echo ""
    
    cd src/ChatbotService
    dotnet run --urls "http://localhost:5000"
fi
