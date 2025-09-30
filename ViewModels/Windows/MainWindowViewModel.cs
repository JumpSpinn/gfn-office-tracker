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
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
	private readonly IServiceProvider _serviceProvider;

	[ObservableProperty]
	private ViewModelBase? _currentPage;

	public MainWindowViewModel(IServiceProvider sp)
	{
		_serviceProvider = sp;

		// the first page is the splash screen
		_currentPage = sp.GetRequiredService<SplashScreenPageViewModel>();

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
			Page.SETUP => _serviceProvider.GetRequiredService<StatsFormViewModel>(),
			Page.MAIN => _serviceProvider.GetRequiredService<MainPageViewModel>(),
			_ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
		};
}
