namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class SplashScreenPageViewModel : ViewModelBase
{
	private readonly IDbContextFactory<OtContext> _dbContextFactory;
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
	private readonly MainWindowController _mainWindowController;
	private readonly LogController _logController;

	public SplashScreenPageViewModel(MainWindowController mwc, LogController lc, IDbContextFactory<OtContext> dbContextFactory)
	{
		_mainWindowController = mwc;
		_logController = lc;
		_dbContextFactory = dbContextFactory;
		_loadQueue.Enqueue(InitializeLoggerAsync);
		_loadQueue.Enqueue(InitializeDatabaseAsync);
		_loadQueue.Enqueue(InitializeAppWindowAsync);
		StartInitializationAsync();
	}

	#region Initialization

	private async Task StartInitializationAsync()
		=> await TriggerNextLoadStep();

	#endregion

	#region QUEUE

	private readonly Queue<Func<Task<bool>>> _loadQueue = new();

	private async Task TriggerNextLoadStep()
	{
		while (_loadQueue.Count > 0)
		{
			LoadingText = "";
			var loadStep = _loadQueue.Dequeue();

			if (await loadStep.Invoke()) continue;

			_logController.Error("Error while initializing application.");
			return;
		}

		_messenger.Send(new SplashScreenSuccessMessage(_hasData));
	}

	#endregion

	#region LOAD STEPS

	private bool _hasData = true;

	private async Task<bool> InitializeAppWindowAsync()
	{
		const uint maxRetries = 20;
		uint retryCount = 0;

		while (retryCount < maxRetries)
		{
			if (App.MainWindow?.IsLoaded == true)
				return await _mainWindowController.Initialize();

			_logController.Debug(App.MainWindow is null ?
				"MainWindow not available yet... Retrying..." :
				"MainWindow available, but not loaded yet... Retrying...");

			await Task.Delay(500);
			retryCount++;
		}

		DisplayInfoBar("Timeout", "MainWindow konnte nicht initialisiert werden.", InfoBarSeverity.Error);
		return false;
	}

	private async Task<bool> InitializeLoggerAsync()
	{
		try
		{
			ShowInfiniteProgressBar = true;
			LoadingText = "Initializing Logger..";

			if (!await _logController.EnsureLogFile())
			{
				DisplayInfoBar("Error", "Error while trying to ensure Log-File.", InfoBarSeverity.Error);
				return false;
			}
			_logController.Info("Logger initialized.");
			ShowInfiniteProgressBar = false;
			return true;
		}
		catch (Exception e)
		{
			DisplayInfoBar("Critical Error", $"While initializing Logger:\n{e.Message}", InfoBarSeverity.Error);
			return false;
		}
	}

	private async Task<bool> InitializeDatabaseAsync()
	{
		try
		{
			ShowInfiniteProgressBar = true;
			LoadingText = "Initializing Database..";

			var context = await _dbContextFactory.CreateDbContextAsync();
			await context.Database.MigrateAsync();
			_hasData = context.General.Any();

			_logController.Info("Database initialized.");
			ShowInfiniteProgressBar = false;
			return true;
		}
		catch (Exception e)
		{
			DisplayInfoBar("Critical Error", $"While initializing Database:\n{e.Message}", InfoBarSeverity.Error);
			return false;
		}
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

	#region VERSION

	[ObservableProperty]
	private string _version = $"Version: {Options.VERSION}";

	#endregion
}
