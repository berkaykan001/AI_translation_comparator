# Claude_Gemini.md

This file provides guidance to Claude and Gemini when working with code in this repository.

## Project Overview

This is a .NET MAUI cross-platform mobile application called "AI Translator Mobile App". The application's primary function is to provide translation services, enhanced with AI-powered grammar checking and suggestions. It integrates with both Claude and Gemini AI models to offer robust translation and language correction capabilities.

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
- **AI_bots/**: Contains all AI service implementations.
  - `BaseChatService.cs`: Abstract base class for AI services.
  - `claude.cs`: Implementation for the Claude AI model.
  - `gemini.cs`: Implementation for the Gemini AI model.
  - `CombinedMethodForChatBots.cs`: Unified interface for all AI services.
  - `LLMConfiguration.cs`: Configuration for API keys and endpoints.
- **Translators/**: Contains the core translation logic.
  - `Translators.cs`: Implements translation functionality, likely integrating with the AI services for advanced features.

### Page Architecture
The app has a simple and focused UI:
- **TranslationPage**: The main interface where users can input text for translation, select languages, and receive translations. This page utilizes the AI models for grammar checking and improving the quality of the translations.

### Key Design Patterns
- **Strategy Pattern**: Different AI services (`claude.cs`, `gemini.cs`) implement a common interface, allowing the application to switch between them or use them together.
- **Factory Pattern**: Used for creating instances of AI services.

### Model Configuration
Models are configured in `LLMConfiguration.cs` with:
- API keys and service-specific endpoints.

## Important Implementation Details

### Multi-Platform Targets
- Android (API 21+)
- iOS (15.0+)
- Windows (10.0.17763.0+)
- macOS Catalyst (15.0+)

### Authentication
API keys are stored in `LLMConfiguration.cs`. For production, these should be moved to a secure storage mechanism.

### Performance Optimization
- Asynchronous programming (`async`/`await`) is used extensively for network calls to AI services to ensure a responsive UI.

### Error Handling
- `try-catch` blocks are used to handle potential errors during API calls.
- User-friendly error messages are displayed to the user.

## Development Notes

### Adding New AI Services
1. Create a new service class in `AI_bots/` that inherits from `BaseChatService`.
2. Add the API configuration to `LLMConfiguration.cs`.
3. Register the new service in `CombinedMethodForChatBots.cs`.
4. Update the `TranslationPage` to utilize the new service if needed.
