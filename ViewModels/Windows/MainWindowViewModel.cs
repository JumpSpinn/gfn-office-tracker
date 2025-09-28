namespace OfficeTracker.ViewModels.Windows;

[RegisterSingleton]
public sealed partial class MainWindowViewModel : ViewModelBase
{
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

	[ObservableProperty]
	private ViewModelBase? _currentPage;

	public MainWindowViewModel(IServiceProvider sp)
	{
		_currentPage = sp.GetRequiredService<SplashScreenPageViewModel>();
		_messenger.Register<MainWindowViewModel, SplashScreenSuccessMessage>(this, (_, _) =>
		{
			// CurrentPage = sp.GetRequiredService<MainPageViewModel>();
			CurrentPage = sp.GetRequiredService<LoginFormViewModel>();
		});
	}
}
