namespace OfficeTracker.ViewModels.Windows;

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

	[ObservableProperty]
	private ViewModelBase? _currentPage;

	public MainWindowViewModel(IServiceProvider sp, LogService lc)
	{
		_serviceProvider = sp;

		// the first page is the splash screen
		_currentPage = sp.GetRequiredService<SplashPageViewModel>();

		_messenger.Register<MainWindowViewModel, ChangePageMessage>(this, (_, message) =>
		{
			CurrentPage = GetCurrentPageViewModel(message.Value);
		});
	}

	/// <summary>
	/// Retrieves the appropriate ViewModel for the specified <see cref="Page"/>.
	/// </summary>
	private ViewModelBase GetCurrentPageViewModel(Page page) =>
		page switch
		{
			Page.WIZARD_WELCOME => _serviceProvider.GetRequiredService<WizardWelcomePageViewModel>(),
			Page.WIZARD_NAME => _serviceProvider.GetRequiredService<WizardNamePageViewModel>(),
			Page.WIZARD_BALANCE => _serviceProvider.GetRequiredService<WizardBalancePageViewModel>(),
			Page.WIZARD_DAYS => _serviceProvider.GetRequiredService<WizardDaysPageViewModel>(),
			Page.WIZARD_DATA => _serviceProvider.GetRequiredService<WizardDataPageViewModel>(),
			Page.WIZARD_COMPLETED => _serviceProvider.GetRequiredService<WizardCompletedPageViewModel>(),
			Page.MAIN => _serviceProvider.GetRequiredService<MainPageViewModel>(),
			_ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
		};
}
