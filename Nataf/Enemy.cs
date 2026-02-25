namespace Nataf;

public sealed class Enemy
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int AttackPower { get; set; }
    public char Symbol { get; set; }
    public string Name { get; set; } = "Enemy";
    public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    public bool IsAlive => Health > 0;
    public int MoveCounter { get; set; }
    public int MoveFrequency { get; set; } = 3;
    public EnemyType Type { get; set; }

    public void TakeDamage(int amount)
    {
        Health = Math.Max(0, Health - amount);
    }

    public static Enemy CreateScorpion(int x, int y) => new()
    {
        X = x, Y = y, Health = 20, MaxHealth = 20, AttackPower = 8,
        Symbol = '♣', Name = "Scorpion", Color = ConsoleColor.Yellow,
        MoveFrequency = 4, Type = EnemyType.Scorpion
    };

    public static Enemy CreateBandit(int x, int y) => new()
    {
        X = x, Y = y, Health = 35, MaxHealth = 35, AttackPower = 12,
        Symbol = '☠', Name = "Desert Bandit", Color = ConsoleColor.Red,
        MoveFrequency = 3, Type = EnemyType.Bandit
    };

    public static Enemy CreateSnake(int x, int y) => new()
    {
        X = x, Y = y, Health = 15, MaxHealth = 15, AttackPower = 18,
        Symbol = '§', Name = "Cobra", Color = ConsoleColor.Green,
        MoveFrequency = 2, Type = EnemyType.Snake
    };

    public static Enemy CreateMummy(int x, int y) => new()
    {
        X = x, Y = y, Health = 60, MaxHealth = 60, AttackPower = 15,
        Symbol = 'M', Name = "Mummy", Color = ConsoleColor.DarkYellow,
        MoveFrequency = 5, Type = EnemyType.Mummy
    };

    public static Enemy CreateDjinn(int x, int y) => new()
    {
        X = x, Y = y, Health = 50, MaxHealth = 50, AttackPower = 20,
        Symbol = '◊', Name = "Djinn", Color = ConsoleColor.Cyan,
        MoveFrequency = 2, Type = EnemyType.Djinn
    };

    public static Enemy CreateDragon(int x, int y) => new()
    {
        X = x, Y = y, Health = 150, MaxHealth = 150, AttackPower = 25,
        Symbol = 'Ð', Name = "Sand Dragon", Color = ConsoleColor.Magenta,
        MoveFrequency = 3, Type = EnemyType.Boss
    };

    public static Enemy CreateSultan(int x, int y) => new()
    {
        X = x, Y = y, Health = 200, MaxHealth = 200, AttackPower = 30,
        Symbol = '♛', Name = "Dark Sultan", Color = ConsoleColor.DarkRed,
        MoveFrequency = 2, Type = EnemyType.Boss
    };
}

public enum EnemyType
{
    Scorpion,
    Bandit,
    Snake,
    Mummy,
    Djinn,
    Boss
}
