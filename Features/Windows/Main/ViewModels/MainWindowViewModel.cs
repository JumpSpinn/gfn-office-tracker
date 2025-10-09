namespace OfficeTracker.Features.Windows.Main.ViewModels;

/// <summary>
/// Represents the main view model for the application's main window.
/// This ViewModel is responsible for managing the navigation logic
/// between different pages of the application, such as the splash screen,
/// main page, and statistics form.
/// </summary>
[RegisterSingleton]
public sealed partial class MainWindowViewModel : ViewModelBase
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ConfigController _configController;

	[ObservableProperty]
	private ViewModelBase? _currentPage;

	public MainWindowViewModel(IServiceProvider sp, ConfigController cc)
	{
		_serviceProvider = sp;
		_configController = cc;
		_currentPage = sp.GetRequiredService<SplashPageViewModel>();

		_messenger.Register<Features.Windows.Main.ViewModels.MainWindowViewModel, ChangePageMessage>(this, (_, message) =>
		{
			SetMainWindowBarVisibility(true);
			CurrentPage = GetCurrentPageViewModel(message.Value);
		});
	}

	/// <summary>
	/// Retrieves the current page's ViewModel based on the provided page type.
	/// </summary>
	/// <param name="page">The page enum value representing the desired UI page.</param>
	/// <returns>A ViewModelBase instance corresponding to the given page type.</returns>
	private ViewModelBase GetCurrentPageViewModel(Page page) =>
		page switch
		{
			Page.WIZARD_WELCOME => _serviceProvider.GetRequiredService<WizardWelcomePageViewModel>(),
			Page.WIZARD_NAME => _serviceProvider.GetRequiredService<WizardNamePageViewModel>(),
			Page.WIZARD_BALANCE => _serviceProvider.GetRequiredService<WizardBalancePageViewModel>(),
			Page.WIZARD_DAYS => _serviceProvider.GetRequiredService<WizardDaysPageViewModel>(),
			Page.WIZARD_DATA => _serviceProvider.GetRequiredService<WizardDataPageViewModel>(),
			Page.WIZARD_COMPLETED => _serviceProvider.GetRequiredService<WizardCompletedPageViewModel>(),
			Page.MAIN_WINDOW => _serviceProvider.GetRequiredService<MainPageViewModel>(),
			Page.SETTINGS_WINDOW => _serviceProvider.GetRequiredService<SettingsPageViewModel>(),
			_ => _serviceProvider.GetRequiredService<NotFoundPageViewModel>()
		};
}
