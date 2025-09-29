namespace OfficeTracker;

/// <summary>
/// Represents the entry point and main class for the OfficeTracker application.
/// This class is responsible for initializing and launching the application.
/// </summary>
internal abstract class Program
{
	/// <summary>
	/// A unique identifier string used for the global mutex in the OfficeTracker application.
	/// This ensures that only one instance of the application runs at a time
	/// by utilizing the system-wide mutex mechanism.
	/// </summary>
	private const string MUTEX_ID = "Global\\OfficeTracker";

	/// <summary>
	/// Entry point of the OfficeTracker application.
	/// This method initializes the application, ensures a single instance is running, and starts Avalonia's desktop lifetime.
	/// </summary>
	[STAThread]
    public static void Main(string[] args)
    {
	    //Checks if there already is an instance of office tracker
        using Mutex mutes = new(false, MUTEX_ID, out bool createdNew);
        if (!createdNew) return;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

	/// <summary>
	/// Configures and builds the Avalonia application instance.
	/// This method sets up the application framework, platform detection, and logging configuration.
	/// </summary>
	public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
