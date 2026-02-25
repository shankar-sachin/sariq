using System.Runtime.InteropServices;

namespace Nataf;

/// <summary>
/// Provides native sound effects via platform-specific calls.
/// On Windows, uses Beep from kernel32. On other platforms, falls back to Console.Beep.
/// The companion C++ NatafNative library can be compiled separately for extended effects.
/// </summary>
public static class NativeSoundEngine
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool Beep(uint frequency, uint duration);

    private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static void PlayAttackSound()
    {
        if (!IsWindows) return;
        Task.Run(() =>
        {
            try
            {
                Beep(800, 50);
                Beep(600, 50);
            }
            catch { }
        });
    }

    public static void PlayHitSound()
    {
        if (!IsWindows) return;
        Task.Run(() =>
        {
            try
            {
                Beep(200, 100);
            }
            catch { }
        });
    }

    public static void PlayPickupSound()
    {
        if (!IsWindows) return;
        Task.Run(() =>
        {
            try
            {
                Beep(1200, 30);
                Beep(1500, 30);
            }
            catch { }
        });
    }

    public static void PlayLevelUpSound()
    {
        if (!IsWindows) return;
        Task.Run(() =>
        {
            try
            {
                Beep(523, 100);
                Beep(659, 100);
                Beep(784, 100);
                Beep(1047, 200);
            }
            catch { }
        });
    }

    public static void PlayDeathSound()
    {
        if (!IsWindows) return;
        Task.Run(() =>
        {
            try
            {
                Beep(400, 200);
                Beep(300, 200);
                Beep(200, 400);
            }
            catch { }
        });
    }

    public static void PlayVictoryFanfare()
    {
        if (!IsWindows) return;
        Task.Run(() =>
        {
            try
            {
                Beep(523, 150);
                Beep(659, 150);
                Beep(784, 150);
                Beep(1047, 300);
                Thread.Sleep(100);
                Beep(784, 150);
                Beep(1047, 400);
            }
            catch { }
        });
    }

    public static void PlayPortalSound()
    {
        if (!IsWindows) return;
        Task.Run(() =>
        {
            try
            {
                for (uint f = 400; f <= 1200; f += 100)
                    Beep(f, 30);
            }
            catch { }
        });
    }
}
