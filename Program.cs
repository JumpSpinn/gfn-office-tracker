using Avalonia;
using System;
using System.Threading;

namespace OfficeTracker;

class Program
{
    private const string MutexId = "Global\\OfficeTracker";
    
    [STAThread]
    public static void Main(string[] args)
    {
        using Mutex mutes = new(false, MutexId, out bool createdNew);
        if (!createdNew) return;
        
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
