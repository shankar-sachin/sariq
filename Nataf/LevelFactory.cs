namespace Nataf;

public sealed class LevelData
{
    public int Number { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public char[,] Map { get; init; } = new char[0, 0];
    public int PlayerStartX { get; init; }
    public int PlayerStartY { get; init; }
    public List<Enemy> Enemies { get; init; } = [];
    public List<Item> Items { get; init; } = [];
    public int PortalX { get; init; }
    public int PortalY { get; init; }
    public ConsoleColor SandColor { get; init; } = ConsoleColor.DarkYellow;
    public ConsoleColor WallColor { get; init; } = ConsoleColor.Gray;
}

public static class LevelFactory
{
    public const int MapWidth = 60;
    public const int MapHeight = 25;

    public static LevelData CreateLevel(int levelNumber) => levelNumber switch
    {
        1 => CreateLevel1(),
        2 => CreateLevel2(),
        3 => CreateLevel3(),
        4 => CreateLevel4(),
        5 => CreateLevel5(),
        _ => CreateLevel1()
    };

    public const int TotalLevels = 5;

    private static LevelData CreateLevel1()
    {
        var map = new char[MapHeight, MapWidth];
        InitializeMap(map, '░');

        // Outer walls
        DrawBorder(map);

        // Inner structures - a bazaar
        DrawHorizontalWall(map, 5, 10, 30);
        DrawHorizontalWall(map, 12, 10, 30);
        DrawVerticalWall(map, 10, 5, 12);
        DrawVerticalWall(map, 20, 5, 12);
        DrawVerticalWall(map, 30, 5, 12);

        // Doorways
        map[8, 10] = '░';
        map[8, 20] = '░';
        map[8, 30] = '░';
        map[5, 15] = '░';
        map[12, 25] = '░';

        // Water feature
        for (int x = 40; x < 55; x++)
        {
            map[18, x] = '≈';
            map[19, x] = '≈';
            map[20, x] = '≈';
        }

        return new LevelData
        {
            Number = 1,
            Name = "The Grand Bazaar",
            Description = "Navigate the bustling bazaar, collect gold, and find the exit portal!",
            Map = map,
            PlayerStartX = 2,
            PlayerStartY = 2,
            Enemies =
            [
                Enemy.CreateScorpion(15, 8),
                Enemy.CreateScorpion(25, 8),
                Enemy.CreateBandit(35, 15),
                Enemy.CreateSnake(45, 10),
            ],
            Items =
            [
                Item.CreateGold(12, 7), Item.CreateGold(22, 7),
                Item.CreateGold(27, 9), Item.CreateGold(16, 10),
                Item.CreatePotion(50, 19),
                Item.CreateGem(35, 3),
                Item.CreateGold(5, 20), Item.CreateGold(48, 5),
            ],
            PortalX = 57,
            PortalY = 22,
            SandColor = ConsoleColor.DarkYellow,
            WallColor = ConsoleColor.Gray
        };
    }

    private static LevelData CreateLevel2()
    {
        var map = new char[MapHeight, MapWidth];
        InitializeMap(map, '░');
        DrawBorder(map);

        // Pyramid interior - corridors
        // Outer triangle hint
        for (int row = 2; row < 22; row++)
        {
            int left = 30 - row;
            int right = 30 + row;
            if (left >= 1 && left < MapWidth - 1) map[row, left] = '▓';
            if (right >= 1 && right < MapWidth - 1) map[row, right] = '▓';
        }

        // Internal maze walls
        DrawHorizontalWall(map, 6, 15, 45);
        map[6, 25] = '░';
        map[6, 35] = '░';

        DrawHorizontalWall(map, 10, 12, 48);
        map[10, 20] = '░';
        map[10, 40] = '░';

        DrawHorizontalWall(map, 14, 10, 50);
        map[14, 30] = '░';

        DrawHorizontalWall(map, 18, 8, 52);
        map[18, 15] = '░';
        map[18, 45] = '░';

        DrawVerticalWall(map, 25, 6, 14);
        map[9, 25] = '░';

        DrawVerticalWall(map, 35, 10, 18);
        map[13, 35] = '░';

        return new LevelData
        {
            Number = 2,
            Name = "Pyramid of Shadows",
            Description = "Explore the ancient pyramid. Beware of mummies and traps!",
            Map = map,
            PlayerStartX = 30,
            PlayerStartY = 22,
            Enemies =
            [
                Enemy.CreateMummy(20, 8),
                Enemy.CreateMummy(40, 8),
                Enemy.CreateScorpion(30, 12),
                Enemy.CreateSnake(15, 16),
                Enemy.CreateSnake(45, 16),
                Enemy.CreateBandit(25, 20),
            ],
            Items =
            [
                Item.CreateGold(30, 4), Item.CreateGem(22, 8),
                Item.CreateGold(38, 12), Item.CreatePotion(12, 16),
                Item.CreateKey(48, 16), Item.CreateGold(30, 20),
                Item.CreatePotion(20, 20), Item.CreateScroll(42, 4),
            ],
            PortalX = 30,
            PortalY = 3,
            SandColor = ConsoleColor.DarkYellow,
            WallColor = ConsoleColor.DarkGray
        };
    }

    private static LevelData CreateLevel3()
    {
        var map = new char[MapHeight, MapWidth];
        InitializeMap(map, '░');
        DrawBorder(map);

        // Oasis with water in the center
        for (int y = 8; y <= 16; y++)
            for (int x = 20; x <= 40; x++)
                map[y, x] = '≈';

        // Islands in the water
        for (int x = 27; x <= 33; x++)
            for (int y = 11; y <= 13; y++)
                map[y, x] = '░';

        // Bridges
        for (int x = 20; x <= 40; x++)
            map[12, x] = '░';
        for (int y = 8; y <= 16; y++)
            map[y, 30] = '░';

        // Palm tree clusters (walls)
        DrawVerticalWall(map, 5, 3, 20);
        DrawVerticalWall(map, 55, 3, 20);
        DrawHorizontalWall(map, 3, 5, 55);
        DrawHorizontalWall(map, 21, 5, 55);

        // Gaps
        map[3, 15] = '░'; map[3, 30] = '░'; map[3, 45] = '░';
        map[21, 15] = '░'; map[21, 30] = '░'; map[21, 45] = '░';
        map[10, 5] = '░'; map[15, 5] = '░';
        map[10, 55] = '░'; map[15, 55] = '░';

        return new LevelData
        {
            Number = 3,
            Name = "Oasis of Mirages",
            Description = "Cross the enchanted oasis. The water hides secrets...",
            Map = map,
            PlayerStartX = 2,
            PlayerStartY = 12,
            Enemies =
            [
                Enemy.CreateDjinn(30, 12),
                Enemy.CreateSnake(25, 12),
                Enemy.CreateSnake(35, 12),
                Enemy.CreateScorpion(10, 5),
                Enemy.CreateScorpion(50, 5),
                Enemy.CreateBandit(10, 18),
                Enemy.CreateBandit(50, 18),
            ],
            Items =
            [
                Item.CreateGold(30, 11), Item.CreateGem(30, 13),
                Item.CreatePotion(10, 10), Item.CreatePotion(50, 10),
                Item.CreateGold(15, 5), Item.CreateGold(45, 5),
                Item.CreateKey(30, 6), Item.CreateCompass(45, 18),
                Item.CreateScroll(15, 18),
            ],
            PortalX = 57,
            PortalY = 12,
            SandColor = ConsoleColor.Yellow,
            WallColor = ConsoleColor.DarkGreen
        };
    }

    private static LevelData CreateLevel4()
    {
        var map = new char[MapHeight, MapWidth];
        InitializeMap(map, '░');
        DrawBorder(map);

        // Sultan's palace - ornate corridors
        // Central hall
        for (int x = 20; x <= 40; x++)
        {
            map[3, x] = '▓';
            map[21, x] = '▓';
        }
        for (int y = 3; y <= 21; y++)
        {
            map[y, 20] = '▓';
            map[y, 40] = '▓';
        }
        map[12, 20] = '░'; map[12, 40] = '░';
        map[3, 30] = '░'; map[21, 30] = '░';

        // Throne room inner walls
        for (int x = 25; x <= 35; x++)
        {
            map[8, x] = '▓';
            map[16, x] = '▓';
        }
        map[8, 30] = '░'; map[16, 30] = '░';

        // Side chambers
        DrawHorizontalWall(map, 8, 3, 18);
        map[8, 10] = '░';
        DrawHorizontalWall(map, 16, 3, 18);
        map[16, 10] = '░';

        DrawHorizontalWall(map, 8, 42, 57);
        map[8, 50] = '░';
        DrawHorizontalWall(map, 16, 42, 57);
        map[16, 50] = '░';

        // Pillars
        map[6, 5] = '▓'; map[6, 15] = '▓';
        map[18, 5] = '▓'; map[18, 15] = '▓';
        map[6, 45] = '▓'; map[6, 55] = '▓';
        map[18, 45] = '▓'; map[18, 55] = '▓';

        return new LevelData
        {
            Number = 4,
            Name = "Sultan's Palace",
            Description = "Infiltrate the Sultan's palace. The Dragon guards the inner sanctum!",
            Map = map,
            PlayerStartX = 2,
            PlayerStartY = 2,
            Enemies =
            [
                Enemy.CreateBandit(10, 5),
                Enemy.CreateBandit(50, 5),
                Enemy.CreateBandit(10, 19),
                Enemy.CreateBandit(50, 19),
                Enemy.CreateDjinn(25, 12),
                Enemy.CreateDjinn(35, 12),
                Enemy.CreateMummy(30, 5),
                Enemy.CreateDragon(30, 12),
            ],
            Items =
            [
                Item.CreateGold(5, 5), Item.CreateGold(55, 5),
                Item.CreateGold(5, 19), Item.CreateGold(55, 19),
                Item.CreatePotion(10, 12), Item.CreatePotion(50, 12),
                Item.CreateGem(27, 10), Item.CreateGem(33, 10),
                Item.CreateKey(30, 20), Item.CreateScroll(30, 3),
            ],
            PortalX = 30,
            PortalY = 12,
            SandColor = ConsoleColor.DarkYellow,
            WallColor = ConsoleColor.DarkMagenta
        };
    }

    private static LevelData CreateLevel5()
    {
        var map = new char[MapHeight, MapWidth];
        InitializeMap(map, '░');
        DrawBorder(map);

        // Final level - Desert of Trials with mixed terrain
        // Lava rivers (shown as special water)
        for (int x = 1; x < MapWidth - 1; x++)
        {
            if (x is < 10 or > 12 and < 25 or > 27 and < 45 or > 47)
            {
                map[8, x] = '≈';
                map[16, x] = '≈';
            }
        }

        // Fortification walls
        DrawVerticalWall(map, 15, 1, 8);
        map[4, 15] = '░';
        DrawVerticalWall(map, 45, 1, 8);
        map[4, 45] = '░';

        DrawVerticalWall(map, 15, 16, 23);
        map[20, 15] = '░';
        DrawVerticalWall(map, 45, 16, 23);
        map[20, 45] = '░';

        // Central arena
        for (int x = 22; x <= 38; x++)
        {
            map[10, x] = '▓';
            map[14, x] = '▓';
        }
        for (int y = 10; y <= 14; y++)
        {
            map[y, 22] = '▓';
            map[y, 38] = '▓';
        }
        map[10, 30] = '░'; map[14, 30] = '░';
        map[12, 22] = '░'; map[12, 38] = '░';

        // Scattered obstacles
        map[4, 8] = '▓'; map[4, 52] = '▓';
        map[20, 8] = '▓'; map[20, 52] = '▓';
        map[3, 30] = '▓'; map[21, 30] = '▓';

        return new LevelData
        {
            Number = 5,
            Name = "Desert of Judgement",
            Description = "The final battle awaits! Defeat the Dark Sultan to claim victory!",
            Map = map,
            PlayerStartX = 2,
            PlayerStartY = 22,
            Enemies =
            [
                Enemy.CreateBandit(8, 3),
                Enemy.CreateBandit(52, 3),
                Enemy.CreateDjinn(8, 20),
                Enemy.CreateDjinn(52, 20),
                Enemy.CreateMummy(20, 5),
                Enemy.CreateMummy(40, 5),
                Enemy.CreateDragon(25, 18),
                Enemy.CreateSnake(35, 18),
                Enemy.CreateSultan(30, 12),
            ],
            Items =
            [
                Item.CreatePotion(5, 5), Item.CreatePotion(55, 5),
                Item.CreatePotion(5, 19), Item.CreatePotion(55, 19),
                Item.CreateGold(11, 8), Item.CreateGold(26, 8),
                Item.CreateGold(46, 8), Item.CreateGem(30, 3),
                Item.CreateScroll(11, 16), Item.CreateScroll(46, 16),
                Item.CreateGem(30, 21), Item.CreateKey(30, 6),
            ],
            PortalX = 30,
            PortalY = 12,
            SandColor = ConsoleColor.DarkRed,
            WallColor = ConsoleColor.DarkGray
        };
    }

    private static void InitializeMap(char[,] map, char fill)
    {
        for (int y = 0; y < MapHeight; y++)
            for (int x = 0; x < MapWidth; x++)
                map[y, x] = fill;
    }

    private static void DrawBorder(char[,] map)
    {
        for (int x = 0; x < MapWidth; x++)
        {
            map[0, x] = '▓';
            map[MapHeight - 1, x] = '▓';
        }
        for (int y = 0; y < MapHeight; y++)
        {
            map[y, 0] = '▓';
            map[y, MapWidth - 1] = '▓';
        }
    }

    private static void DrawHorizontalWall(char[,] map, int row, int startX, int endX)
    {
        for (int x = startX; x <= Math.Min(endX, MapWidth - 1); x++)
            map[row, x] = '▓';
    }

    private static void DrawVerticalWall(char[,] map, int col, int startY, int endY)
    {
        for (int y = startY; y <= Math.Min(endY, MapHeight - 1); y++)
            map[y, col] = '▓';
    }
}
