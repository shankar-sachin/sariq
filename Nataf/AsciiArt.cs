namespace Nataf;

public static class AsciiArt
{
    public const string TitleScreen = """

         ███████╗ █████╗ ██████╗ ██╗ ██████╗ 
         ██╔════╝██╔══██╗██╔══██╗██║██╔═══██╗
         ███████╗███████║██████╔╝██║██║   ██║
         ╚════██║██╔══██║██╔══██╗██║██║▄▄ ██║
         ███████║██║  ██║██║  ██║██║╚██████╔╝
         ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝ ╚══▀▀═╝

              ╔══════════════════════════════╗
              ║  THIEF OF THE ANCIENT SANDS  ║
              ╠══════════════════════════════╣
              ║                              ║
              ║   Steal. Fight. Survive.     ║
              ║                              ║
              ╚══════════════════════════════╝

                       .     *    .   *
                   *       .         .
                 .    *        *
             ~         ~         ~
          ~~   ~~   ~~   ~~   ~~   ~~
        /\    /\    /\    /\    /\    /\
       /  \  /  \  /  \  /  \  /  \  /  \
      / .. \/ .. \/ .. \/ .. \/ .. \/ .. \
     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        [ENTER] Start   [H] Help   [Q] Quit
    """;

    public const string HelpScreen = """

       ╔═══════════════════════════════════════╗
       ║         HOW TO PLAY SARIQ             ║
       ╠═══════════════════════════════════════╣
       ║                                       ║
       ║  MOVEMENT:                            ║
       ║  W/↑ - Up   S/↓ - Down               ║
       ║  A/← - Left D/→ - Right              ║
       ║                                       ║
       ║  ACTIONS:                             ║
       ║  SPACE - Attack (in combat)           ║
       ║  F     - Flee   (in combat)           ║
       ║  E     - Use Health Potion            ║
       ║  ESC   - Return to Title              ║
       ║                                       ║
       ║  RULES:                               ║
       ║  • Walk into enemies to fight them    ║
       ║  • Defeat ALL enemies to open portal  ║
       ║  • Collect gold (♦) and gems (◆)      ║
       ║  • Potions (♥) heal 40 HP             ║
       ║  • Scrolls (▪) boost your attack      ║
       ║  • Water (≈) hurts! Lava is worse!    ║
       ║  • Complete quests for bonus rewards   ║
       ║                                       ║
       ║  SYMBOLS:                             ║
       ║  ⚔  You    ♦  Gold    ♥  Potion      ║
       ║  ☠  Bandit ♣  Scorpion §  Cobra      ║
       ║  M  Mummy  ◊  Djinn   Ð  Dragon      ║
       ║  ♛  Sultan ▓  Wall    Ω  Portal       ║
       ║                                       ║
       ╚═══════════════════════════════════════╝

              [Press any key to go back]
    """;

    public const string GameOverScreen = """

         ╔══════════════════════════════════╗
         ║                                  ║
         ║          ☠  GAME OVER  ☠         ║
         ║                                  ║
         ║    The desert claims another     ║
         ║    thief...                      ║
         ║                                  ║
         ║    [R] Retry   [Q] Quit          ║
         ║                                  ║
         ╚══════════════════════════════════╝

               .  *  . *  .  *  . *
            ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
           /\  /\  /\  /\  /\  /\  /\  /\
          ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    """;

    public const string VictoryScreen = """

         ╔══════════════════════════════════╗
         ║                                  ║
         ║       ★  VICTORY!  ★             ║
         ║                                  ║
         ║   The greatest thief in all      ║
         ║   the land! You plundered the    ║
         ║   Sultan's legendary treasure!   ║
         ║                                  ║
         ║   [ENTER] Play Again  [Q] Quit   ║
         ║                                  ║
         ╚══════════════════════════════════╝

            *  . ★ .  *  . ★ .  *  . ★
              .    *     .    *     .
    """;

    public static readonly string[] CamelFrames =
    [
        """
           __    __
          /  \__/  \
         |  (oo)   |
          \  --  __/
           |    |/
          /|    |\
         /_|    |_\
        """,
        """
           __    __
          /  \__/  \
         |  (oo)   |
          \  --  __/
           |    |/
          /|    | \
         / |    |  \
        """
    ];

    public const string Pyramid = """
              /\
             /  \
            / ◊  \
           /      \
          / ♦  ♦   \
         /    ☠     \
        /  ♦     ♦   \
       /_______________\
    """;

    public const string Bazaar = """
         _______________
        |  __|_____|__  |
        | |  BAZAAR  | |
        | | ♦  ♦  ♦ | |
        | |  SPICES  | |
        | |___♦__♦___| |
        |_____| |_______|
              | |
          ~~~~   ~~~~
    """;

    public const string Palace = """
            ┌──┐ ┌──┐
          ┌─┤  ├─┤  ├─┐
         ┌┤ │  │ │  │ ├┐
         │├─┤◊◊├─┤◊◊├─┤│
         ││ │  │Ω│  │ ││
         │├─┴──┴─┴──┴─┤│
         │ ▓▓▓▓▓▓▓▓▓▓▓ │
         └─────────────┘
    """;

    public const string Oasis = """
         .  * . *  .
        ╱ ╲   ╱ ╲   ╱ ╲
       ╱   ╲ ╱   ╲ ╱   ╲
       |   | |   | |   |
       ≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈
       ≈≈≈≈≈≈♥≈≈≈≈≈≈≈≈≈≈
       ≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈≈
    """;

    public const string Dragon = """
              /\    .-" /
             /  ; .'  .' 
            :   :/  .'   
             \  ;-.'     
          jgs`.  `.  \    
               `.  "  \   
                 `.  (  \  
                   `. ;  |
                     `   /
    """;

    public const string LevelBanner = """
       ╔══════════════════════════════════╗
       ║         LEVEL {0}: {1,-15}  ║
       ╚══════════════════════════════════╝
    """;
}
