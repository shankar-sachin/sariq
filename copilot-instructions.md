# Copilot Instructions - Sariq: Thief of the Ancient Sands

## Project Overview
**Sariq** (Arabic for Thief) is a Middle East-themed ASCII console adventure game built with C# (.NET 10) and a C++ native library. The player is a desert thief navigating 5 levels.

## Architecture
- Program.cs - Entry point
- GameEngine.cs - Main loop, state machine, ANSI escape setup, quest integration
- Player.cs - Player state (icon: crossed swords)
- Enemy.cs - 7 enemy types with factory methods
- EnemyAI.cs - Per-type AI behaviors
- Item.cs - Collectibles: Gold, Potion, Key, Compass, Gem, Scroll
- LevelFactory.cs - 5 hand-crafted levels
- Renderer.cs - Buffer-based ANSI renderer (flicker-free)
- CombatSystem.cs - Turn-based combat with crits/dodges/flee
- NativeSoundEngine.cs - Sound effects via kernel32 Beep
- AsciiArt.cs - All ASCII art assets
- QuestSystem.cs - Per-level quest definitions and tracking

## Rendering (Flicker-Free)
- Uses ANSI escape sequences in a StringBuilder
- Single Console.Write call per frame
- EnableAnsiEscapes() enables VT processing on Windows

## Quest System
Each level has 3 quests: KillAll, CollectGold, CollectItems, ReachPortal, FindKey, SurviveAboveHp

## Conventions
- .NET 10, C# 14, C++17
- Top-level statements, file-scoped namespaces, collection expressions
- Player icon is crossed swords - no religious symbols
- Static factory methods for Enemies/Items

## Building
dotnet run --project Nataf/Nataf.csproj
