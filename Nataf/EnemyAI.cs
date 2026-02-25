namespace Nataf;

public static class EnemyAI
{
    private static readonly Random Rng = new();

    public static void UpdateEnemies(LevelData level, Player player)
    {
        foreach (var enemy in level.Enemies)
        {
            if (!enemy.IsAlive) continue;

            enemy.MoveCounter++;
            if (enemy.MoveCounter < enemy.MoveFrequency) continue;
            enemy.MoveCounter = 0;

            int dx = 0, dy = 0;

            double distance = Math.Sqrt(Math.Pow(player.X - enemy.X, 2) + Math.Pow(player.Y - enemy.Y, 2));

            switch (enemy.Type)
            {
                case EnemyType.Scorpion:
                    // Random movement
                    (dx, dy) = Rng.Next(4) switch
                    {
                        0 => (0, -1),
                        1 => (0, 1),
                        2 => (-1, 0),
                        _ => (1, 0)
                    };
                    break;

                case EnemyType.Snake:
                    // Chase if close
                    if (distance < 8)
                        (dx, dy) = ChasePlayer(enemy, player);
                    else
                        (dx, dy) = RandomMove();
                    break;

                case EnemyType.Bandit:
                    // Chase player when within range
                    if (distance < 12)
                        (dx, dy) = ChasePlayer(enemy, player);
                    else
                        (dx, dy) = RandomMove();
                    break;

                case EnemyType.Mummy:
                    // Slow but always chases
                    (dx, dy) = ChasePlayer(enemy, player);
                    break;

                case EnemyType.Djinn:
                    // Teleport-like movement (large jumps)
                    if (distance < 10 && Rng.Next(100) < 30)
                    {
                        dx = Math.Sign(player.X - enemy.X) * 2;
                        dy = Math.Sign(player.Y - enemy.Y) * 2;
                    }
                    else
                        (dx, dy) = ChasePlayer(enemy, player);
                    break;

                case EnemyType.Boss:
                    // Always chase, sometimes fast
                    (dx, dy) = ChasePlayer(enemy, player);
                    if (Rng.Next(100) < 20)
                    {
                        dx *= 2;
                        dy *= 2;
                    }
                    break;
            }

            int newX = enemy.X + dx;
            int newY = enemy.Y + dy;

            if (IsValidMove(level, newX, newY) && !(newX == player.X && newY == player.Y))
            {
                enemy.X = newX;
                enemy.Y = newY;
            }
        }
    }

    private static (int dx, int dy) ChasePlayer(Enemy enemy, Player player)
    {
        int dx = Math.Sign(player.X - enemy.X);
        int dy = Math.Sign(player.Y - enemy.Y);

        // Prefer the axis with the larger distance
        if (Math.Abs(player.X - enemy.X) > Math.Abs(player.Y - enemy.Y))
            return (dx, 0);
        else if (Math.Abs(player.Y - enemy.Y) > Math.Abs(player.X - enemy.X))
            return (0, dy);
        else
            return Rng.Next(2) == 0 ? (dx, 0) : (0, dy);
    }

    private static (int dx, int dy) RandomMove()
    {
        return Rng.Next(4) switch
        {
            0 => (0, -1),
            1 => (0, 1),
            2 => (-1, 0),
            _ => (1, 0)
        };
    }

    private static bool IsValidMove(LevelData level, int x, int y)
    {
        if (x < 0 || x >= LevelFactory.MapWidth || y < 0 || y >= LevelFactory.MapHeight)
            return false;

        char tile = level.Map[y, x];
        return tile is not ('▓' or '≈');
    }
}
