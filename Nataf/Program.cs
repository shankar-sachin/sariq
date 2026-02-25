using Nataf;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CursorVisible = false;

try
{
    Console.SetWindowSize(Math.Min(120, Console.LargestWindowWidth), Math.Min(40, Console.LargestWindowHeight));
    Console.SetBufferSize(Math.Min(120, Console.LargestWindowWidth), Math.Min(40, Console.LargestWindowHeight));
}
catch { /* Not supported on all terminals */ }

var game = new GameEngine();
game.Run();