# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET MAUI cross-platform mobile application called "AI Brainstorm" that provides AI model comparison and translation services. The app integrates with multiple AI services including OpenAI, Claude, Mistral, Perplexity, Gemini, DeepSeek, Grok, and various translation services.

## Build and Development Commands

### Build Commands
```bash
# Build for all platforms
dotnet build

# Build for specific platform
dotnet build -f net9.0-android
dotnet build -f net9.0-ios
dotnet build -f net9.0-windows10.0.19041.0
dotnet build -f net9.0-maccatalyst

# Release build for Android
dotnet build -c Release -f net9.0-android

# Publish for Android
dotnet publish -c Release -f net9.0-android
```

### Running the Application
```bash
# Run on Windows
dotnet run -f net9.0-windows10.0.19041.0

# For mobile platforms, use Visual Studio or deploy to emulator/device
```

### Package Management
```bash
# Restore NuGet packages
dotnet restore

# Clean build artifacts
dotnet clean
```

## Project Architecture

### Core Structure
- **AI_bots/**: Contains all AI service implementations
  - `BaseChatService.cs`: Abstract base class providing conversation management, token estimation, and cost calculation
  - Individual service classes for each AI provider (OpenAI, Claude, Mistral, etc.)
  - `CombinedMethodForChatBots.cs`: Unified interface for all AI services
  - `LLMConfiguration.cs`: Configuration for API keys, endpoints, and model pricing

### Page Architecture
The app uses a tabbed interface with specialized pages:
- **TranslationPage**: Translation services + AI grammar checking
- **QuickResponsePage**: Fast responses using lightweight models
- **ReasoningPage**: Complex reasoning using premium models
- **InternetSearchPage**: Web search capabilities
- **CodingPage**: Code generation and programming assistance
- **GeneratePicturePage**: Image generation from multiple providers
- **ChooseModelPage**: Single model selection with follow-up conversation

### Key Design Patterns
1. **Template Method Pattern**: `BaseAIComparisonPage` provides common functionality for multi-model comparison pages
2. **Strategy Pattern**: Different AI services implement common interfaces
3. **Factory Pattern**: Model mappings and service selection
4. **Observer Pattern**: UI updates via `MainThread.BeginInvokeOnMainThread`

### Model Configuration
Models are configured with:
- Cost per million tokens (input/output)
- Service-specific endpoints and authentication
- Specialized cost calculation for search-enabled models (Perplexity, OpenAI Search)

### Conversation Management
- Per-model conversation history with configurable memory size
- System message management for different use cases
- Automatic conversation trimming to prevent token limit issues

## Important Implementation Details

### Multi-Platform Targets
- Android (API 21+)
- iOS (15.0+)
- Windows (10.0.17763.0+)
- macOS Catalyst (15.0+)

### Authentication
API keys are stored in `LLMConfiguration.cs`. In production, these should be moved to secure storage or environment variables.

### Performance Optimization
- Parallel task execution for multiple AI service calls
- Async/await patterns throughout
- UI thread marshaling for responsive interface

### Error Handling
- Comprehensive try-catch blocks in UI event handlers
- Service-level error handling with fallback mechanisms
- User-friendly error messages via DisplayAlert

## Development Notes

### Adding New AI Services
1. Create new service class in `AI_bots/` inheriting from `BaseChatService`
2. Add API configuration to `LLMConfiguration.cs`
3. Register in `CombinedMethodForChatBots.cs`
4. Update model mappings in relevant pages

### Adding New Pages
1. Create XAML page and code-behind
2. For multi-model comparison, inherit from `BaseAIComparisonPage`
3. Add to `AppShell.xaml` tab navigation
4. Implement required abstract methods

### Cost Calculation
Special handling for:
- Perplexity models: Include search request costs
- OpenAI Search models: Context-size dependent pricing
- Image generation: Per-image pricing rather than token-based

### Platform-Specific Features
- Android: Uses APK packaging with keystore signing
- iOS: Requires privacy manifest and entitlements
- Windows: MSIX packaging for distribution