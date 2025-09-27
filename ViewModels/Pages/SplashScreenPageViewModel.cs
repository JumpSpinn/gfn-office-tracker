namespace OfficeTracker.ViewModels.Pages;

using Services.Windows.Controllers;

[RegisterSingleton]
public sealed partial class SplashScreenPageViewModel : ViewModelBase
{
	private readonly MainWindowController _mainWindowController;
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

	public SplashScreenPageViewModel(MainWindowController mwc)
	{
		_mainWindowController = mwc;
		_loadQueue.Enqueue(InitializeAppWindowAsync);
		// _loadQueue.Enqueue(InitializeDebugAsync);
		TriggerNextLoadStep();
	}

	#region QUEUE

	private readonly Queue<Func<Task<bool>>> _loadQueue = new();

	private async Task TriggerNextLoadStep()
	{
		LoadingText = "";
		if (_loadQueue.Count > 0)
		{
			LoadingText = "";
			if (await _loadQueue.Dequeue().Invoke())
				TriggerNextLoadStep();
			else
				return;
		}
		else
			_messenger.Send(new SplashScreenSuccessMessage(true));
	}

	#endregion

	#region LOAD STEPS

	private async Task<bool> InitializeAppWindowAsync()
	{
		while (true)
		{
			if (App.MainWindow is null)
			{
				await Task.Delay(100);
				continue;
			}

			if (!App.MainWindow.IsLoaded)
			{
				await Task.Delay(500);
				continue;
			}

			return await _mainWindowController.Initialize();
		}
	}

	private async Task<bool> InitializeDebugAsync()
	{
		LoadingText = "Lade Daten vom File..";
		await Task.Delay(5000);

		LoadingText = "Berechne Daten..";
		await Task.Delay(5000);

		LoadingText = "Lade eingetragene Tage..";
		await Task.Delay(5000);

		LoadingText = "Berechne Statistiken..";
		await Task.Delay(5000);

		return true;
	}

	#endregion

	#region TEXT

	[ObservableProperty]
	private string _loadingText = "Starte Office-Tracker..";

	#endregion

	#region INFO BAR

	[ObservableProperty]
	private bool _showInfoBar;

	[ObservableProperty]
	private string _infoBarTitle = string.Empty;

	[ObservableProperty]
	private string _infoBarText = string.Empty;

	[ObservableProperty]
	private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;

	private void DisplayInfoBar(string title, string text, InfoBarSeverity severity)
	{
		ShowInfoBar = true;
		InfoBarTitle = title;
		InfoBarText = text;
		InfoBarSeverity = severity;
	}

	#endregion

	#region INFINITE PROGRESSBAR

	[ObservableProperty]
	private bool _showInfiniteProgressBar;

	#endregion
}
