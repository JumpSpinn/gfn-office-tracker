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
		_messenger.Register<MainWindowViewModel, SplashScreenSuccessMessage>(this, (_, success) =>
		{
			if(success.Value)
				CurrentPage = sp.GetRequiredService<MainPageViewModel>();
			else
				CurrentPage = sp.GetRequiredService<StatsFormViewModel>();
		});

		_messenger.Register<MainWindowViewModel, StatsFormSuccessMessage>(this, (_, success) =>
		{
			if(success.Value)
				CurrentPage = sp.GetRequiredService<MainPageViewModel>();
			else
				CurrentPage = sp.GetRequiredService<StatsFormViewModel>();
		});
	}
}
