namespace OfficeTracker;

/// <summary>
/// Represents the main application class for the OfficeTracker application.
/// </summary>
public sealed partial class App : Application
{
	/// <summary>
	/// Gets the application's configured service provider.
	/// </summary>
	public IServiceProvider? Services { get; private set; }

	/// <summary>
	/// Initializes the application's XAML resources and components.
	/// This method is called during the application startup to load the XAML configuration.
	/// </summary>
	public override void Initialize()
        =>  AvaloniaXamlLoader.Load(this);

	/// <summary>
	/// Gets or sets the list of command-line arguments passed to the application during startup.
	/// </summary>
	public static List<string> StartupArgs { get; set; } = [];

	/// <summary>
	/// Gets or sets the main application window instance for the OfficeTracker application.
	/// </summary>
	public static Window? MainWindow { get; set; }

	/// <summary>
	/// Finalizes the initialization of the application framework and sets up the application's main window and services.
	/// This method is invoked after the application framework completes its setup process.
	/// It configures services, processes startup arguments, and establishes the main window of the application.
	/// </summary>
	public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        DisableAvaloniaDataAnnotationValidation();

        if(desktop.Args is not null)
            StartupArgs = desktop.Args.ToList();

        Services = new ServiceCollection()
	        .AutoRegister()
            .AddDbContext<OtContext>(options =>
            {
#if DEBUG
	            options
		            .UseSqlite($"Data Source={Options.DB_NAME}")
		            .EnableSensitiveDataLogging()
		            .EnableDetailedErrors()
		            .LogTo(Console.WriteLine, LogLevel.Information);
#else
	            options
		            .UseSqlite($"Data Source={Options.DB_NAME}")
		            .EnableDetailedErrors();
#endif
            })
            .AddSingleton<IDbContextFactory<OtContext>, OtContextFactory>()
            .BuildServiceProvider();

        var vm = Services.GetRequiredService<MainWindowViewModel>();
        MainWindow = desktop.MainWindow = new MainWindow()
        {
            DataContext = vm
        };

        base.OnFrameworkInitializationCompleted();
    }

	/// <summary>
	/// Disables Avalonia data annotation validation to prevent duplicate validation errors.
	/// This method removes validation plugins related to data annotations from Avalonia's binding
	/// system, particularly useful when using additional validation libraries like CommunityToolkit.
	/// </summary>
	private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
            BindingPlugins.DataValidators.Remove(plugin);
    }
}
