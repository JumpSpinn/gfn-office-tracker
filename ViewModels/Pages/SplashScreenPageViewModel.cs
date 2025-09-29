namespace OfficeTracker.ViewModels.Pages;

/// <summary>
/// Represents the ViewModel for the splash screen page of the application.
/// Responsible for initializing the application's essential services and operations
/// during startup.
/// </summary>
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
		StartInitializationAsync();
	}

	#region Initialization

	/// <summary>
	/// Starts the initialization process of the application's essential services and operations.
	/// This method enqueues initialization steps such as logger setup, database initialization,
	/// and application window preparation, and triggers their execution in sequence asynchronously.
	/// </summary>
	private async Task StartInitializationAsync()
	{
		_loadQueue.Enqueue(InitializeLoggerAsync);
		_loadQueue.Enqueue(InitializeDatabaseAsync);
		_loadQueue.Enqueue(InitializeAppWindowAsync);
		await TriggerNextLoadStep();
	}

	#endregion

	#region QUEUE

	private readonly Queue<Func<Task<bool>>> _loadQueue = new();

	/// <summary>
	/// Processes and executes the next initialization step from the load queue.
	/// This method retrieves and invokes tasks sequentially from the initialization queue.
	/// If a task fails, an error is logged, and the process halts. Upon successful execution
	/// of all steps, it sends a success message indicating the completion of the splash screen procedures.
	/// </summary>
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

	private bool _hasData;

	/// <summary>
	/// Attempts to initialize the application's main window asynchronously.
	/// This method ensures the main window is loaded and then delegates its initialization
	/// to the MainWindowController. If the initialization fails after a set number of retries,
	/// an error message is displayed to the user.
	/// </summary>
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

	/// <summary>
	/// Initializes the logging framework for the application asynchronously.
	/// Ensures that the log file is properly created and ready for use.
	/// Updates progress indicators during the process, and logs an informational message upon success.
	/// Displays an error notification in case of failure during initialization.
	/// </summary>
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

	/// <summary>
	/// Asynchronously initializes the database by creating its context, applying any pending migrations,
	/// and determining whether it contains any data. Handles logging and displays progress information
	/// during the initialization process.
	/// </summary>
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

	/// <summary>
	/// Displays an information bar with the specified title, text, and severity level.
	/// This method is used to provide user-facing feedback about the application's
	/// current state or encountered issues.
	/// </summary>
	/// <param name="title">The title of the information bar message.</param>
	/// <param name="text">The detailed text content of the message to be displayed in the information bar.</param>
	/// <param name="severity">The severity level of the message, determining the visual style and importance of the information bar.</param>
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
