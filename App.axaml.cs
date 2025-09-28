namespace OfficeTracker;

using Database.Factories;

public sealed partial class App : Application
{
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
        =>  AvaloniaXamlLoader.Load(this);

    public static List<string> StartupArgs { get; set; } = [];

    public static Window? MainWindow { get; set; }

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
            .AddDbContext<OfContext>(options
	            => options.UseSqlite($"Data Source={Options.DB_NAME}"))
            .AddSingleton<IDbContextFactory<OfContext>, OfContextFactory>()
            .BuildServiceProvider();

        var vm = Services.GetRequiredService<MainWindowViewModel>();
        MainWindow = desktop.MainWindow = new MainWindow()
        {
            DataContext = vm
        };

        base.OnFrameworkInitializationCompleted();
    }

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
