namespace OfficeTracker.ViewModels.Windows;

[RegisterSingleton]
public sealed partial class MainWindowViewModel : ViewModelBase
{
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

	[ObservableProperty]
	private ViewModelBase? _currentPage;

	public MainWindowViewModel(IServiceProvider serviceProvider)
	{
		_currentPage = serviceProvider.GetRequiredService<SplashScreenPageViewModel>();
		_messenger.Register<MainWindowViewModel, SplashScreenSuccessMessage>(this, (_, _) =>
		{
			CurrentPage = serviceProvider.GetRequiredService<MainPageViewModel>();
		});
	}
}
