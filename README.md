# Sariq: Thief of the Ancient Sands

A Middle East-themed ASCII console adventure game built with C# (.NET 10). Play as a desert thief navigating 5 hand-crafted levels filled with enemies, loot, and quests.

## Gameplay

- Explore tile-based levels rendered in the terminal with ANSI colors
- Fight 7 enemy types in turn-based combat with critical hits, dodges, and flee options
- Collect gold, potions, keys, gems, scrolls, and compasses
- Complete 3 quests per level to progress
- Native sound effects via Windows Beep API

## Controls

| Key | Action |
|-----|--------|
| W A S D | Move |
| E | Interact / Open chest |
| F | Fight / Attack |
| R | Run / Flee combat |
| I | Inventory |
| Q | Quit |

## Building & Running

Requires [.NET 10 SDK](https://dotnet.microsoft.com/download).

```
dotnet run --project Nataf/Nataf.csproj
```

## Architecture

| File | Purpose |
|------|---------|
| `Program.cs` | Entry point |
| `GameEngine.cs` | Main loop, state machine, quest integration |
| `Player.cs` | Player state |
| `Enemy.cs` | 7 enemy types with factory methods |
| `EnemyAI.cs` | Per-type AI behaviours |
| `Item.cs` | Collectible items |
| `LevelFactory.cs` | 5 hand-crafted levels |
| `Renderer.cs` | Buffer-based ANSI renderer (flicker-free) |
| `CombatSystem.cs` | Turn-based combat |
| `QuestSystem.cs` | Per-level quest definitions and tracking |
| `NativeSoundEngine.cs` | Sound effects via `kernel32 Beep` |
| `AsciiArt.cs` | ASCII art assets |

## Requirements

- Windows (for ANSI VT processing and native sound)
- .NET 10
