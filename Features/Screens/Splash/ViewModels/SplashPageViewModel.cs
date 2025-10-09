namespace OfficeTracker.Features.Screens.Splash.ViewModels;

/// <summary>
/// Represents the ViewModel for the splash screen page of the application.
/// Responsible for initializing the application's essential services and operations
/// during startup.
/// </summary>
[RegisterSingleton]
public sealed partial class SplashPageViewModel : ViewModelBase
{
	private readonly IDbContextFactory<OtContext> _dbContextFactory;
	private readonly MainWindowController _mainWindowController;
	private readonly ConfigController _configController;
	private readonly LogController _logController;

	public SplashPageViewModel(MainWindowController mwc, LogController lc, IDbContextFactory<OtContext> dbContextFactory, ConfigController cc)
	{
		_mainWindowController = mwc;
		_logController = lc;
		_dbContextFactory = dbContextFactory;
		_configController = cc;
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
		_loadQueue.Enqueue(InitializeConfigAsync);
		_loadQueue.Enqueue(InitializeDatabaseAsync);
		_loadQueue.Enqueue(LoadRuntimeDataAsync);
		_loadQueue.Enqueue(InitializeAppWindowAsync);
		_loadQueue.Enqueue(InitializeDialogsAsync);
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

		ChangePage(_hasData ? Page.MAIN_WINDOW : Page.WIZARD_WELCOME);
	}

	#endregion

	#region LOAD STEPS

	/// <summary>
	/// Indicates whether the database contains existing data.
	/// This variable is used during the application's initialization phase
	/// to determine whether the main page (if data is present) or
	/// the wizard welcome page (if no data is present) should be displayed
	/// after the splash screen.
	/// </summary>
	private bool _hasData;

	/// <summary>
	/// Indicates whether the dialogs required for the application's startup sequence
	/// have been preloaded. This variable assists in managing the initialization process
	/// by ensuring that essential dialogs are available and ready for display during
	/// the application's splash screen phase.
	/// </summary>
	private bool _dialogsPreloaded;

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

			await using var db = await _dbContextFactory.CreateDbContextAsync();
			await db.Database.MigrateAsync();
			_hasData = db.UserSettings.Any();

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

	private async Task<bool> InitializeDialogsAsync()
	{
		try
		{
			if (_dialogsPreloaded) return true;

			ShowInfiniteProgressBar = false;
			LoadingText = "Initializing Dialogs..";

			var totalDialogs = Enum.GetValues<DialogType>().Length;
			var dialogsInitialized = 0;

			InitializeValueProgress(dialogsInitialized, totalDialogs);

			foreach (var type in Enum.GetValues<DialogType>())
			{
				var container = DialogHelper.CreateContentContainer(type);
				var dialogContent = DialogHelper.CreateContentWithIcon(container, type, "Prewarming..");
				var dialog = new ContentDialog()
				{
					Title = "Prewarming...",
					PrimaryButtonText = "Ok",
					DefaultButton = ContentDialogButton.Close,
					Content = dialogContent,
					Opacity = 0,
				};

				var t = dialog.ShowAsync(App.MainWindow);
				await Task.Yield();
				dialog.Hide();

				try
				{
					await t;
					dialogsInitialized++;
					ValueProgress = dialogsInitialized;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}

			_logController.Info("Dialogs initialized.");
			ShowValueProgress = false;
			_dialogsPreloaded = true;
			return true;
		}
		catch (Exception e)
		{
			DisplayInfoBar("Critical Error", $"While initializing Dialogs:\n{e.Message}", InfoBarSeverity.Error);
			return false;
		}
	}

	/// <summary>
	/// Loads necessary runtime data for the application asynchronously. This includes retrieving
	/// user-specific settings from the database and preparing the application for runtime operations.
	/// </summary>
	private async Task<bool> LoadRuntimeDataAsync()
	{
		try
		{
			ShowInfiniteProgressBar = true;
			LoadingText = "Check for Runtime Data..";

			await using var db = await _dbContextFactory.CreateDbContextAsync();
			var userSettings = await db.UserSettings.FirstOrDefaultAsync();
			if (userSettings is not null)
			{
				LoadingText = "Runtime Data found. Loading..";
				_mainWindowController.SetRuntimeDataAsync(userSettings);
				_logController.Info("Runtime Data was loaded.");
			}

			ShowInfiniteProgressBar = false;
			return true;
		}
		catch (Exception e)
		{
			DisplayInfoBar("Critical Error", $"While loading Runtime Data:\n{e.Message}", InfoBarSeverity.Error);
			return false;
		}
	}

	/// <summary>
	/// Asynchronously initializes the application's configuration settings by invoking the necessary configuration services.
	/// Handles the display of loading indicators and logs the status of the initialization process.
	/// Reports any critical errors encountered during the initialization.
	/// </summary>
	private async Task<bool> InitializeConfigAsync()
	{
		try
		{
			ShowInfiniteProgressBar = true;
			LoadingText = "Initializing Config..";

			if (!await _configController.InitializeConfigAsync())
			{
				DisplayInfoBar("Error", "Error while trying to initialize Config.", InfoBarSeverity.Error);
				return false;
			}

			_logController.Info("Config initialized.");
			ShowInfiniteProgressBar = false;
			return true;
		}
		catch (Exception e)
		{
			DisplayInfoBar("Critical Error", $"While initializing Config:\n{e.Message}", InfoBarSeverity.Error);
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
	private void DisplayInfoBar(string title, string text, InfoBarSeverity severity)
	{
		ShowInfoBar = true;
		InfoBarTitle = title;
		InfoBarText = text;
		InfoBarSeverity = severity;
		_logController.Info(title, text);
	}

	#endregion

	#region INFINITE PROGRESSBAR

	[ObservableProperty]
	private bool _showInfiniteProgressBar;

	#endregion

	#region VALUE PROGRESSBAR
	[ObservableProperty]
	private bool _showValueProgress;

	[ObservableProperty]
	private double _valueProgress;

	[ObservableProperty]
	private double _valueProgressMin;

	[ObservableProperty]
	private double _valueProgressMax = 100;

	/// <summary>
	/// Initializes the progress value tracking with specified minimum and maximum values.
	/// Configures the current progress value and enables the display of progress information.
	/// </summary>
	private void InitializeValueProgress(double min, double max)
	{
		ValueProgress = min;
		ValueProgressMin = min;
		ValueProgressMax = max;
		ShowValueProgress = true;
	}
	#endregion

	#region VERSION

	[ObservableProperty]
	private string _version = $"Version: {Options.VERSION}";

	#endregion
}
