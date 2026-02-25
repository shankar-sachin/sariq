using System.Text;

namespace Nataf;

/// <summary>
/// Buffer-based renderer that builds each frame in a StringBuilder and writes
/// it in a single Console.Write call to eliminate flicker.
/// </summary>
public static class Renderer
{
    private static readonly object RenderLock = new();
    private static readonly StringBuilder Buf = new(8192);

    // ANSI escape helpers — works on Windows 10+ and all modern terminals
    private static string Fg(ConsoleColor c) => c switch
    {
        ConsoleColor.Black       => "\x1b[30m",
        ConsoleColor.DarkBlue    => "\x1b[34m",
        ConsoleColor.DarkGreen   => "\x1b[32m",
        ConsoleColor.DarkCyan    => "\x1b[36m",
        ConsoleColor.DarkRed     => "\x1b[31m",
        ConsoleColor.DarkMagenta => "\x1b[35m",
        ConsoleColor.DarkYellow  => "\x1b[33m",
        ConsoleColor.Gray        => "\x1b[37m",
        ConsoleColor.DarkGray    => "\x1b[90m",
        ConsoleColor.Blue        => "\x1b[94m",
        ConsoleColor.Green       => "\x1b[92m",
        ConsoleColor.Cyan        => "\x1b[96m",
        ConsoleColor.Red         => "\x1b[91m",
        ConsoleColor.Magenta     => "\x1b[95m",
        ConsoleColor.Yellow      => "\x1b[93m",
        ConsoleColor.White       => "\x1b[97m",
        _                        => "\x1b[37m"
    };

    private const string BgDarkBlue = "\x1b[44m";
    private const string BgReset    = "\x1b[49m";
    private const string Reset      = "\x1b[0m";

    public static void DrawTitle()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(AsciiArt.TitleScreen);
        Console.ResetColor();
    }

    public static void DrawHelp()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(AsciiArt.HelpScreen);
        Console.ResetColor();
    }

    public static void DrawGameOver(int score)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(AsciiArt.GameOverScreen);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"          Final Score: {score}");
        Console.ResetColor();
    }

    public static void DrawVictory(int score, int gold)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(AsciiArt.VictoryScreen);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"          Final Score: {score}   Gold: {gold}");
        Console.ResetColor();
    }

    public static void DrawLevelIntro(LevelData level, List<Quest> quests)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;

        string banner = string.Format(AsciiArt.LevelBanner, level.Number, level.Name);
        Console.WriteLine(banner);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine($"       {level.Description}");
        Console.WriteLine();

        // Show level-appropriate art
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        string art = level.Number switch
        {
            1 => AsciiArt.Bazaar,
            2 => AsciiArt.Pyramid,
            3 => AsciiArt.Oasis,
            4 => AsciiArt.Palace,
            5 => AsciiArt.Dragon,
            _ => AsciiArt.Pyramid
        };
        Console.WriteLine(art);

        // Show quests for this level
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("       ┌─── QUESTS ───────────────────────────┐");
        foreach (var q in quests)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"       │  ► ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{q.Title,-38}│");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"       │    {q.Description,-36}│");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"       │    Reward: +{q.RewardGold}g +{q.RewardScore}pts{"",16}│");
        }
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("       └─────────────────────────────────────┘");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\n       [Press any key to begin...]");
        Console.ResetColor();
        Console.ReadKey(true);
    }

    public static void DrawGame(LevelData level, Player player, string message, List<Quest> quests)
    {
        lock (RenderLock)
        {
            Buf.Clear();
            Buf.Append("\x1b[H"); // Move cursor to top-left (no clear = no flicker)

            BuildHUD(Buf, player, level, quests);
            BuildMap(Buf, level, player);
            BuildMessage(Buf, message);

            Buf.Append(Reset);
            Console.Write(Buf);
        }
    }

    private static void BuildHUD(StringBuilder sb, Player player, LevelData level, List<Quest> quests)
    {
        sb.Append(Fg(ConsoleColor.DarkCyan));
        sb.Append('╔').Append('═', 58).Append("╗\n");

        sb.Append("║ ");
        sb.Append(Fg(ConsoleColor.White));
        sb.Append($"Level {level.Number}: {level.Name,-20}");
        sb.Append(Fg(ConsoleColor.Yellow));
        sb.Append($"Score: {player.Score,-8}");
        sb.Append($"Gold: {player.Gold,-5}");
        sb.Append(Fg(ConsoleColor.DarkCyan));
        sb.Append("║\n");

        sb.Append("║ ");
        sb.Append(Fg(ConsoleColor.White));
        sb.Append("HP: ");
        int healthBars = (int)((double)player.Health / player.MaxHealth * 20);
        var hpColor = healthBars > 14 ? ConsoleColor.Green :
                      healthBars > 7  ? ConsoleColor.Yellow : ConsoleColor.Red;
        sb.Append(Fg(hpColor));
        sb.Append('█', healthBars).Append('░', 20 - healthBars);
        sb.Append(Fg(ConsoleColor.White));
        sb.Append($" {player.Health,3}/{player.MaxHealth}");
        sb.Append($"  Potions:{player.Potions} Keys:{player.Keys}  ");
        sb.Append(Fg(ConsoleColor.DarkCyan));
        sb.Append("║\n");

        // Quest tracker row
        var activeQuest = quests.FirstOrDefault(q => !q.Claimed);
        sb.Append("║ ");
        sb.Append(Fg(ConsoleColor.Cyan));
        if (activeQuest is not null)
        {
            string tag = activeQuest.IsComplete ? "✓ " : "► ";
            string progress = activeQuest.Type == QuestType.SurviveAboveHp
                ? $"(HP≥{activeQuest.TargetCount})"
                : $"({activeQuest.CurrentCount}/{activeQuest.TargetCount})";
            string questLine = $"{tag}{activeQuest.Title} {progress}";
            sb.Append(questLine.PadRight(57));
        }
        else
        {
            sb.Append("All quests complete!".PadRight(57));
        }
        sb.Append(Fg(ConsoleColor.DarkCyan));
        sb.Append("║\n");

        sb.Append('╚').Append('═', 58).Append("╝\n");
    }

    private static void BuildMap(StringBuilder sb, LevelData level, Player player)
    {
        var map = level.Map;
        int height = map.GetLength(0);
        int width = map.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == player.X && y == player.Y)
                {
                    sb.Append(Fg(ConsoleColor.White)).Append(BgDarkBlue);
                    sb.Append(player.Symbol);
                    sb.Append(BgReset);
                    continue;
                }

                var enemy = level.Enemies.Find(e => e.IsAlive && e.X == x && e.Y == y);
                if (enemy is not null)
                {
                    sb.Append(Fg(enemy.Color)).Append(enemy.Symbol);
                    continue;
                }

                var item = level.Items.Find(i => !i.Collected && i.X == x && i.Y == y);
                if (item is not null)
                {
                    sb.Append(Fg(item.Color)).Append(item.Symbol);
                    continue;
                }

                if (x == level.PortalX && y == level.PortalY)
                {
                    bool allDead = level.Enemies.TrueForAll(e => !e.IsAlive);
                    sb.Append(Fg(allDead ? ConsoleColor.Green : ConsoleColor.DarkGray));
                    sb.Append('Ω');
                    continue;
                }

                char tile = map[y, x];
                sb.Append(tile switch
                {
                    '▓' => Fg(level.WallColor),
                    '≈' => Fg(level.Number == 5 ? ConsoleColor.Red : ConsoleColor.Blue),
                    '░' => Fg(level.SandColor),
                    _   => Fg(ConsoleColor.DarkYellow)
                });
                sb.Append(tile);
            }
            sb.Append('\n');
        }
    }

    private static void BuildMessage(StringBuilder sb, string message)
    {
        sb.Append(Fg(ConsoleColor.DarkCyan));
        sb.Append('╔').Append('═', 58).Append("╗\n");
        sb.Append("║ ");
        sb.Append(Fg(ConsoleColor.White));
        sb.Append(message.Length > 57 ? message[..57] : message.PadRight(57));
        sb.Append(Fg(ConsoleColor.DarkCyan));
        sb.Append("║\n");
        sb.Append('╚').Append('═', 58).Append("╝\n");
    }

    public static void DrawCombatOverlay(Player player, Enemy enemy)
    {
        int boxTop = 10; // shifted down slightly to account for quest HUD row
        int boxLeft = 15;

        lock (RenderLock)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int row = 0; row < 10; row++)
            {
                Console.SetCursorPosition(boxLeft, boxTop + row);
                if (row == 0)
                    Console.Write("╔══════════════════════════╗");
                else if (row == 9)
                    Console.Write("╚══════════════════════════╝");
                else
                {
                    string line = row switch
                    {
                        1 => $"║  ⚔  COMBAT!  ⚔           ║",
                        2 => $"║                           ║",
                        3 => $"║  {enemy.Name,-24} ║",
                        4 => $"║  HP: {enemy.Health,3}/{enemy.MaxHealth,-3}              ║",
                        5 => $"║                           ║",
                        6 => $"║  Your HP: {player.Health,3}/{player.MaxHealth,-3}         ║",
                        7 => $"║                           ║",
                        8 => $"║  [SPACE] Attack  [F] Flee ║",
                        _ => $"║                           ║"
                    };
                    Console.Write(line);
                }
            }
            Console.ResetColor();
        }
    }
}
