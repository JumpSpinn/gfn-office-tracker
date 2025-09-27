namespace OfficeTracker;

internal abstract class Program
{
    private const string MUTEX_ID = "Global\\OfficeTracker";

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
	    //Checks if there already is an instance of office tracker
        using Mutex mutes = new(false, MUTEX_ID, out bool createdNew);
        if (!createdNew) return;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
