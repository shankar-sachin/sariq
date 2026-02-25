namespace Nataf;

public sealed class Player
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
    public int AttackPower { get; set; } = 15;
    public int Gold { get; set; }
    public int Score { get; set; }
    public int Potions { get; set; } = 1;
    public int Keys { get; set; }
    public bool HasCompass { get; set; }
    public char Symbol { get; set; } = '⚔';

    public bool IsAlive => Health > 0;

    public void TakeDamage(int amount)
    {
        Health = Math.Max(0, Health - amount);
    }

    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
    }

    public void UsePotion()
    {
        if (Potions > 0)
        {
            Potions--;
            Heal(40);
        }
    }
}
