namespace Nataf;

public sealed class GameEngine
{
    private Player _player = new();
    private LevelData? _currentLevel;
    private int _currentLevelNumber = 1;
    private string _statusMessage = "Welcome to Sariq! Use WASD or arrows to move.";
    private bool _inCombat;
    private Enemy? _combatEnemy;
    private readonly Random _rng = new();
    private GameState _state = GameState.Title;
    private int _tickCount;
    private List<Quest> _quests = [];
    private int _levelGoldStart; // gold at start of level for quest tracking

    public void Run()
    {
        EnableAnsiEscapes();
        Console.Clear();
        while (true)
        {
            switch (_state)
            {
                case GameState.Title:
                    RunTitleScreen();
                    break;
                case GameState.Playing:
                    RunGameLoop();
                    break;
                case GameState.GameOver:
                    RunGameOver();
                    break;
                case GameState.Victory:
                    RunVictory();
                    break;
            }
        }
    }

    private static void EnableAnsiEscapes()
    {
        if (!OperatingSystem.IsWindows()) return;
        try
        {
            var handle = GetStdHandle(-11); // STD_OUTPUT_HANDLE
            GetConsoleMode(handle, out uint mode);
            SetConsoleMode(handle, mode | 0x0004); // ENABLE_VIRTUAL_TERMINAL_PROCESSING
        }
        catch { /* Best effort */ }
    }

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint GetStdHandle(int nStdHandle);
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

    private void RunTitleScreen()
    {
        Renderer.DrawTitle();

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter:
                    StartNewGame();
                    return;
                case ConsoleKey.H:
                    Renderer.DrawHelp();
                    Console.ReadKey(true);
                    Renderer.DrawTitle();
                    break;
                case ConsoleKey.Q:
                    Console.Clear();
                    Console.CursorVisible = true;
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void StartNewGame()
    {
        _player = new Player();
        _currentLevelNumber = 1;
        LoadLevel(_currentLevelNumber);
        _state = GameState.Playing;
    }

    private void LoadLevel(int levelNumber)
    {
        _currentLevel = LevelFactory.CreateLevel(levelNumber);
        _player.X = _currentLevel.PlayerStartX;
        _player.Y = _currentLevel.PlayerStartY;
        _statusMessage = $"Entering {_currentLevel.Name}... check quests!";
        _inCombat = false;
        _combatEnemy = null;
        _quests = QuestSystem.GetQuestsForLevel(levelNumber);
        _levelGoldStart = _player.Gold;

        Renderer.DrawLevelIntro(_currentLevel, _quests);
        NativeSoundEngine.PlayLevelUpSound();
        Console.Clear();
    }

    private void RunGameLoop()
    {
        if (_currentLevel is null) return;

        Renderer.DrawGame(_currentLevel, _player, _statusMessage, _quests);

        while (_state == GameState.Playing)
        {
            if (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);

                if (_inCombat && _combatEnemy is not null)
                {
                    HandleCombatInput(keyInfo.Key);
                }
                else
                {
                    HandleMovementInput(keyInfo.Key);
                }

                _tickCount++;

                // Enemy AI update every few ticks
                if (_tickCount % 2 == 0 && !_inCombat)
                {
                    EnemyAI.UpdateEnemies(_currentLevel, _player);
                    CheckEnemyCollision();
                }

                // Update and check quests
                if (_currentLevel is not null)
                {
                    QuestSystem.UpdateQuests(_quests, _player, _currentLevel);
                    var questMsg = QuestSystem.TryClaimQuest(_quests, _player);
                    if (questMsg is not null)
                    {
                        _statusMessage = questMsg;
                        NativeSoundEngine.PlayPickupSound();
                    }
                }

                if (_state == GameState.Playing)
                {
                    Renderer.DrawGame(_currentLevel, _player, _statusMessage, _quests);
                    if (_inCombat && _combatEnemy is not null)
                        Renderer.DrawCombatOverlay(_player, _combatEnemy);
                }
            }

            Thread.Sleep(30);
        }
    }

    private void HandleMovementInput(ConsoleKey key)
    {
        if (_currentLevel is null) return;

        int dx = 0, dy = 0;

        switch (key)
        {
            case ConsoleKey.W or ConsoleKey.UpArrow: dy = -1; break;
            case ConsoleKey.S or ConsoleKey.DownArrow: dy = 1; break;
            case ConsoleKey.A or ConsoleKey.LeftArrow: dx = -1; break;
            case ConsoleKey.D or ConsoleKey.RightArrow: dx = 1; break;
            case ConsoleKey.E:
                UseItem();
                return;
            case ConsoleKey.Escape:
                _state = GameState.Title;
                return;
        }

        if (dx == 0 && dy == 0) return;

        int newX = _player.X + dx;
        int newY = _player.Y + dy;

        if (!IsValidPlayerMove(newX, newY)) return;

        // Check for water damage
        if (_currentLevel.Map[newY, newX] == '≈')
        {
            if (_currentLevel.Number == 5)
            {
                _player.TakeDamage(10);
                _statusMessage = "The lava burns! -10 HP";
                NativeSoundEngine.PlayHitSound();
            }
            else
            {
                _player.TakeDamage(3);
                _statusMessage = "You wade through water. -3 HP";
            }

            if (!_player.IsAlive)
            {
                NativeSoundEngine.PlayDeathSound();
                _state = GameState.GameOver;
                return;
            }
        }

        _player.X = newX;
        _player.Y = newY;

        // Check for item pickup
        CheckItemPickup();

        // Check for enemy collision
        CheckEnemyCollision();

        // Check for portal
        CheckPortal();
    }

    private bool IsValidPlayerMove(int x, int y)
    {
        if (_currentLevel is null) return false;
        if (x < 0 || x >= LevelFactory.MapWidth || y < 0 || y >= LevelFactory.MapHeight)
            return false;
        return _currentLevel.Map[y, x] != '▓';
    }

    private void CheckItemPickup()
    {
        if (_currentLevel is null) return;

        var item = _currentLevel.Items.Find(i => !i.Collected && i.X == _player.X && i.Y == _player.Y);
        if (item is null) return;

        item.Collected = true;
        NativeSoundEngine.PlayPickupSound();

        switch (item.Type)
        {
            case ItemType.Gold:
                int goldAmount = _rng.Next(5, 20);
                _player.Gold += goldAmount;
                _player.Score += goldAmount * 2;
                _statusMessage = $"Found {goldAmount} gold! Total: {_player.Gold}";
                break;
            case ItemType.Potion:
                _player.Potions++;
                _statusMessage = $"Found a Health Potion! Potions: {_player.Potions}";
                break;
            case ItemType.Key:
                _player.Keys++;
                _statusMessage = $"Found an Ancient Key! Keys: {_player.Keys}";
                break;
            case ItemType.Compass:
                _player.HasCompass = true;
                _statusMessage = "Found the Magic Compass! Portal location revealed!";
                break;
            case ItemType.Gem:
                _player.Score += 100;
                _player.Gold += 25;
                _statusMessage = "Found a Desert Gem! +100 score +25 gold!";
                break;
            case ItemType.Scroll:
                _player.AttackPower += 5;
                _player.Score += 50;
                _statusMessage = $"Power Scroll! Attack increased to {_player.AttackPower}!";
                break;
        }
    }

    private void CheckEnemyCollision()
    {
        if (_currentLevel is null) return;

        var enemy = _currentLevel.Enemies.Find(e =>
            e.IsAlive && Math.Abs(e.X - _player.X) <= 1 && Math.Abs(e.Y - _player.Y) <= 1);

        if (enemy is not null)
        {
            _inCombat = true;
            _combatEnemy = enemy;
            _statusMessage = $"A {enemy.Name} attacks! [SPACE] to fight, [F] to flee!";
            NativeSoundEngine.PlayAttackSound();
        }
    }

    private void HandleCombatInput(ConsoleKey key)
    {
        if (_combatEnemy is null || !_combatEnemy.IsAlive)
        {
            _inCombat = false;
            _combatEnemy = null;
            return;
        }

        switch (key)
        {
            case ConsoleKey.Spacebar:
            {
                NativeSoundEngine.PlayAttackSound();
                var result = CombatSystem.ProcessCombat(_player, _combatEnemy);
                _statusMessage = result.Message;

                if (result.EnemyKilled)
                {
                    _inCombat = false;
                    _combatEnemy = null;
                }
                else if (result.PlayerDied)
                {
                    NativeSoundEngine.PlayDeathSound();
                    _state = GameState.GameOver;
                }
                break;
            }
            case ConsoleKey.F:
            {
                string fleeResult = CombatSystem.AttemptFlee();
                if (fleeResult == "FLED")
                {
                    _inCombat = false;
                    _combatEnemy = null;
                    // Move player away
                    int dx = _rng.Next(-2, 3);
                    int dy = _rng.Next(-2, 3);
                    int newX = Math.Clamp(_player.X + dx, 1, LevelFactory.MapWidth - 2);
                    int newY = Math.Clamp(_player.Y + dy, 1, LevelFactory.MapHeight - 2);
                    if (_currentLevel is not null && _currentLevel.Map[newY, newX] != '▓')
                    {
                        _player.X = newX;
                        _player.Y = newY;
                    }
                    _statusMessage = "You fled from combat!";
                }
                else
                {
                    _player.TakeDamage(_combatEnemy.AttackPower / 2);
                    _statusMessage = $"Failed to flee! {_combatEnemy.Name} strikes for {_combatEnemy.AttackPower / 2}!";
                    NativeSoundEngine.PlayHitSound();
                    if (!_player.IsAlive)
                    {
                        NativeSoundEngine.PlayDeathSound();
                        _state = GameState.GameOver;
                    }
                }
                break;
            }
            case ConsoleKey.E:
                UseItem();
                break;
        }
    }

    private void UseItem()
    {
        if (_player.Potions > 0)
        {
            _player.UsePotion();
            _statusMessage = $"Used a potion! HP: {_player.Health}/{_player.MaxHealth}";
            NativeSoundEngine.PlayPickupSound();
        }
        else
        {
            _statusMessage = "No potions left!";
        }
    }

    private void CheckPortal()
    {
        if (_currentLevel is null) return;

        if (_player.X != _currentLevel.PortalX || _player.Y != _currentLevel.PortalY) return;

        bool allEnemiesDead = _currentLevel.Enemies.TrueForAll(e => !e.IsAlive);
        if (!allEnemiesDead)
        {
            _statusMessage = "Portal is sealed! Defeat all enemies first!";
            return;
        }

        NativeSoundEngine.PlayPortalSound();
        _player.Score += 200;

        if (_currentLevelNumber >= LevelFactory.TotalLevels)
        {
            NativeSoundEngine.PlayVictoryFanfare();
            _state = GameState.Victory;
        }
        else
        {
            _currentLevelNumber++;
            // Bonus between levels
            _player.Heal(20);
            _player.MaxHealth += 10;
            LoadLevel(_currentLevelNumber);
            Console.Clear();
        }
    }

    private void RunGameOver()
    {
        Renderer.DrawGameOver(_player.Score);

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.R:
                    StartNewGame();
                    return;
                case ConsoleKey.Q:
                    _state = GameState.Title;
                    return;
            }
        }
    }

    private void RunVictory()
    {
        Renderer.DrawVictory(_player.Score, _player.Gold);

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter:
                    StartNewGame();
                    return;
                case ConsoleKey.Q:
                    _state = GameState.Title;
                    return;
            }
        }
    }
}

public enum GameState
{
    Title,
    Playing,
    GameOver,
    Victory
}
