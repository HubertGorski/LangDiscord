# LangDiscord - Language Learning Discord Bot

[![Build Status](https://dev.azure.com/hubertgorski181/HubProjects/_apis/build/status%2FHubProjects?branchName=main)](https://dev.azure.com/hubertgorski181/HubProjects/_build/latest?definitionId=1&branchName=main)

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Integrations](#integrations)
- [Key Pipeline Stages](#build-and-deployment)
- [Installation and configuration](#installation-and-configuration)
- [Available commands](#available-commands)

## Introduction
LangDiscord is a .NET 8-based Discord bot designed to assist with language learning and translation. It allows users to translate text, manage their language preferences, and practice using flashcards—all directly from Discord.

## Features
✅ Translation between multiple languages

📚 Flashcard learning system for vocabulary practice

💾 Favorite phrases collection (via heart reaction)

⚙️ Customizable user preferences (main & favorite language)

🔤 Supports many languages via ISO language codes

💬 Simple command-based interface on Discord

## Integrations
The app integrates with:

- MyMemory

- Tatoeba

## Key Pipeline Stages
✅ Test: Verifies formatting and runs unit tests.

🔧 Build: Optionally builds a Docker image (langdiscord:latest).

🚀 Deploy: Supports install, uninstall, and reinstall of a Docker container using environment variables.

Sensitive configuration files (appsettings.secret.local.json, appsettings.local.json) are downloaded securely during deployment.

## Installation and configuration
### Local Development
```bash
# Restore dependencies
dotnet restore LangDiscord/LangDiscord.csproj

# Run the application
dotnet run --project LangDiscord
```

### Configuration files
Two configuration files are required:

appsettings.local.json

appsettings.secret.local.json

These should be placed in the LangDiscord/ directory. Minimal example:

#### appsettings.local.json
```json
{
    "DiscordChannelId": "1354868876852199515",
    "CacheSettings": {
        "FavoriteTranslationCacheCleanupInterval": "6.00:0:00",
        "FavoriteTranslationCacheDuration": "6.00:00:00",
        "TranslationCacheCleanupInterval": "00:30:00",
        "TranslationCacheDuration": "00:30:00",
        "SettingsCacheDuration": "1.00:00:00",
        "SettingsCacheCleanupInterval": "10:00:00",
        "AllTranslationsCacheDuration": "6.00:00:00",
        "AllTranslationsCacheCleanupInterval": "6.00:00:00"
    }
}
```

#### appsettings.secret.local.json
```json
{
    "DiscordToken": "your-discord-token",
    "DetectLangApiKey": "your-detect-lang-api-key"
}
```

## Available commands

!help - Displays all available bot commands
!config - Displays current settings
!! {text} - Returns translation in main/favorite language
!{langCode} {text} - Translates text to specified language
!{langCode} - Displays a flashcard in given language
!{langCode} u - Displays a favorite flashcard
!fav {langCode} - Sets favorite language
!main {langCode} - Sets main language
!langs - Displays available languages

❤️ Adding a heart reaction to a message saves it to favorites.
- Removing from favorites is done using the built-in button in the bot's message.

### Examples

`User: !pol` - command entered by the user
`Bot: Witaj Świecie!` - bot immediately responds with the first side of a flashcard
Waiting for any user response
`User: Hello` - user's reply, does not need to be a correct translation
`Bot: Hello World!` - correct translation

