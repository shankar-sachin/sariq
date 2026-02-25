namespace Nataf;

public static class CombatSystem
{
    private static readonly Random Rng = new();

    public static CombatResult ProcessCombat(Player player, Enemy enemy)
    {
        // Player attacks
        int playerDamage = Rng.Next(player.AttackPower / 2, player.AttackPower + 1);
        bool criticalHit = Rng.Next(100) < 15;
        if (criticalHit) playerDamage = (int)(playerDamage * 1.8);

        enemy.TakeDamage(playerDamage);

        if (!enemy.IsAlive)
        {
            int goldReward = enemy.Type switch
            {
                EnemyType.Boss => Rng.Next(30, 60),
                EnemyType.Djinn => Rng.Next(15, 30),
                EnemyType.Mummy => Rng.Next(10, 25),
                _ => Rng.Next(5, 15)
            };

            int scoreReward = enemy.Type switch
            {
                EnemyType.Boss => 500,
                EnemyType.Djinn => 200,
                EnemyType.Mummy => 150,
                EnemyType.Bandit => 100,
                _ => 50
            };

            player.Gold += goldReward;
            player.Score += scoreReward;

            string msg = criticalHit
                ? $"CRITICAL! You dealt {playerDamage} damage and slew the {enemy.Name}! +{goldReward}g +{scoreReward}pts"
                : $"You dealt {playerDamage} damage and slew the {enemy.Name}! +{goldReward}g +{scoreReward}pts";

            return new CombatResult(true, false, msg);
        }

        // Enemy attacks back
        int enemyDamage = Rng.Next(enemy.AttackPower / 2, enemy.AttackPower + 1);
        bool dodge = Rng.Next(100) < 10;
        if (dodge) enemyDamage = 0;

        player.TakeDamage(enemyDamage);

        string message = criticalHit
            ? $"CRITICAL {playerDamage} dmg! {enemy.Name} hits back for {enemyDamage}. "
            : dodge
                ? $"You dealt {playerDamage}. You dodged the {enemy.Name}'s attack!"
                : $"You dealt {playerDamage} dmg. {enemy.Name} hits you for {enemyDamage}.";

        return new CombatResult(false, !player.IsAlive, message);
    }

    public static string AttemptFlee()
    {
        bool success = Rng.Next(100) < 60;
        return success ? "FLED" : "FLEE_FAIL";
    }
}

public record CombatResult(bool EnemyKilled, bool PlayerDied, string Message);
