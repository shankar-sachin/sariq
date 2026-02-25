namespace Nataf;

public sealed class Item
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
    public string Name { get; set; } = "Item";
    public ConsoleColor Color { get; set; } = ConsoleColor.White;
    public ItemType Type { get; set; }
    public bool Collected { get; set; }

    public static Item CreateGold(int x, int y) => new()
    {
        X = x, Y = y, Symbol = '♦', Name = "Gold",
        Color = ConsoleColor.Yellow, Type = ItemType.Gold
    };

    public static Item CreatePotion(int x, int y) => new()
    {
        X = x, Y = y, Symbol = '♥', Name = "Health Potion",
        Color = ConsoleColor.Red, Type = ItemType.Potion
    };

    public static Item CreateKey(int x, int y) => new()
    {
        X = x, Y = y, Symbol = '¶', Name = "Ancient Key",
        Color = ConsoleColor.Cyan, Type = ItemType.Key
    };

    public static Item CreateCompass(int x, int y) => new()
    {
        X = x, Y = y, Symbol = '⊕', Name = "Magic Compass",
        Color = ConsoleColor.Magenta, Type = ItemType.Compass
    };

    public static Item CreateGem(int x, int y) => new()
    {
        X = x, Y = y, Symbol = '◆', Name = "Desert Gem",
        Color = ConsoleColor.DarkCyan, Type = ItemType.Gem
    };

    public static Item CreateScroll(int x, int y) => new()
    {
        X = x, Y = y, Symbol = '▪', Name = "Power Scroll",
        Color = ConsoleColor.DarkYellow, Type = ItemType.Scroll
    };
}

public enum ItemType
{
    Gold,
    Potion,
    Key,
    Compass,
    Gem,
    Scroll
}
