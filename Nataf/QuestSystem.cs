namespace Nataf;

public sealed class Quest
{
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public QuestType Type { get; init; }
    public int TargetCount { get; init; } = 1;
    public int CurrentCount { get; set; }
    public bool IsComplete => CurrentCount >= TargetCount;
    public int RewardGold { get; init; }
    public int RewardScore { get; init; }
    public bool Claimed { get; set; }
}

public enum QuestType
{
    KillAll,
    CollectGold,
    CollectItems,
    ReachPortal,
    FindKey,
    SurviveAboveHp
}

public static class QuestSystem
{
    public static List<Quest> GetQuestsForLevel(int levelNumber) => levelNumber switch
    {
        1 =>
        [
            new Quest
            {
                Title = "Clear the Bazaar",
                Description = "Defeat all 4 enemies lurking in the market",
                Type = QuestType.KillAll, TargetCount = 4,
                RewardGold = 20, RewardScore = 150
            },
            new Quest
            {
                Title = "Thief's Haul",
                Description = "Collect at least 30 gold from the stalls",
                Type = QuestType.CollectGold, TargetCount = 30,
                RewardGold = 10, RewardScore = 100
            },
            new Quest
            {
                Title = "Find the Exit",
                Description = "Reach the portal (Ω) after clearing enemies",
                Type = QuestType.ReachPortal, TargetCount = 1,
                RewardGold = 0, RewardScore = 200
            },
        ],
        2 =>
        [
            new Quest
            {
                Title = "Tomb Raider",
                Description = "Defeat all 6 guardians of the pyramid",
                Type = QuestType.KillAll, TargetCount = 6,
                RewardGold = 30, RewardScore = 200
            },
            new Quest
            {
                Title = "Ancient Key",
                Description = "Find the Ancient Key hidden in the depths",
                Type = QuestType.FindKey, TargetCount = 1,
                RewardGold = 15, RewardScore = 100
            },
            new Quest
            {
                Title = "Loot the Pharaoh",
                Description = "Collect 50+ gold from the pyramid chambers",
                Type = QuestType.CollectGold, TargetCount = 50,
                RewardGold = 20, RewardScore = 150
            },
        ],
        3 =>
        [
            new Quest
            {
                Title = "Oasis Ambush",
                Description = "Defeat all 7 enemies around the oasis",
                Type = QuestType.KillAll, TargetCount = 7,
                RewardGold = 35, RewardScore = 250
            },
            new Quest
            {
                Title = "Treasure Diver",
                Description = "Collect 5+ items from the oasis area",
                Type = QuestType.CollectItems, TargetCount = 5,
                RewardGold = 20, RewardScore = 150
            },
            new Quest
            {
                Title = "Survivor",
                Description = "Reach the portal with 50+ HP remaining",
                Type = QuestType.SurviveAboveHp, TargetCount = 50,
                RewardGold = 25, RewardScore = 200
            },
        ],
        4 =>
        [
            new Quest
            {
                Title = "Storm the Palace",
                Description = "Defeat all 8 guards including the Dragon!",
                Type = QuestType.KillAll, TargetCount = 8,
                RewardGold = 50, RewardScore = 350
            },
            new Quest
            {
                Title = "Royal Heist",
                Description = "Steal 80+ gold from the Sultan's vaults",
                Type = QuestType.CollectGold, TargetCount = 80,
                RewardGold = 30, RewardScore = 200
            },
            new Quest
            {
                Title = "Key to the Throne",
                Description = "Find the Key to unlock the inner sanctum",
                Type = QuestType.FindKey, TargetCount = 1,
                RewardGold = 20, RewardScore = 150
            },
        ],
        5 =>
        [
            new Quest
            {
                Title = "Slay the Dark Sultan",
                Description = "Defeat all 9 enemies and the final boss!",
                Type = QuestType.KillAll, TargetCount = 9,
                RewardGold = 75, RewardScore = 500
            },
            new Quest
            {
                Title = "Master Thief",
                Description = "Collect 100+ total gold in the final level",
                Type = QuestType.CollectGold, TargetCount = 100,
                RewardGold = 50, RewardScore = 300
            },
            new Quest
            {
                Title = "Untouchable",
                Description = "Finish with 60+ HP — true mastery!",
                Type = QuestType.SurviveAboveHp, TargetCount = 60,
                RewardGold = 40, RewardScore = 400
            },
        ],
        _ => []
    };

    public static void UpdateQuests(List<Quest> quests, Player player, LevelData level)
    {
        int deadEnemies = level.Enemies.Count(e => !e.IsAlive);
        int collectedItems = level.Items.Count(i => i.Collected);

        foreach (var quest in quests)
        {
            if (quest.Claimed) continue;

            quest.CurrentCount = quest.Type switch
            {
                QuestType.KillAll => deadEnemies,
                QuestType.CollectGold => player.Gold,
                QuestType.CollectItems => collectedItems,
                QuestType.ReachPortal => (player.X == level.PortalX && player.Y == level.PortalY
                    && level.Enemies.TrueForAll(e => !e.IsAlive)) ? 1 : 0,
                QuestType.FindKey => player.Keys > 0 ? 1 : 0,
                QuestType.SurviveAboveHp => player.Health,
                _ => quest.CurrentCount
            };
        }
    }

    public static string? TryClaimQuest(List<Quest> quests, Player player)
    {
        foreach (var quest in quests)
        {
            if (quest.IsComplete && !quest.Claimed)
            {
                quest.Claimed = true;
                player.Gold += quest.RewardGold;
                player.Score += quest.RewardScore;
                return $"QUEST DONE: {quest.Title}! +{quest.RewardGold}g +{quest.RewardScore}pts";
            }
        }
        return null;
    }
}
