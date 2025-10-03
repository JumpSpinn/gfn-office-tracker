namespace OfficeTracker.Services;

/// <summary>
/// Controller responsible for managing interactions and initialization of the main application window.
/// Provides methods to initialize and validate the state of the main application window.
/// </summary>
[RegisterSingleton]
public sealed class MainWindowService(LogService logService, DatabaseService databaseService)
{
	private bool _initialized;

	/// <summary>
	/// Initializes the main application window by verifying its availability and loaded state.
	/// Updates the internal state to mark the controller as initialized upon success.
	/// Logs errors or exceptional cases if initialization fails.
	/// </summary>
	public async Task<bool> Initialize()
	{
		try
		{
			if (_initialized) return true;

			if (App.MainWindow is null)
			{
				logService.Error("MainWindow not available yet. Can't initialize controller.");
				return false;
			}

			if (!App.MainWindow.IsLoaded)
			{
				logService.Error("MainWindow available, but not loaded yet. Can't initialize controller.");
				return false;
			}

			_initialized = true;
			return true;
		}
		catch (Exception ex)
		{
			logService.Exception(ex);
			return false;
		}
	}

	#region RUNTIME DATA

	/// <summary>
	/// Indicates whether the runtime data has been initialized.
	/// This flag is used to prevent reinitialization of runtime data in the application.
	/// </summary>
	private bool _runtimeDataInitialized;

	public RuntimeData RuntimeData { get; private set; } = new();

	/// <summary>
	/// Asynchronously sets the runtime data by fetching user-specific settings from the database.
	/// Verifies if the runtime data has already been initialized to avoid redundant operations.
	/// Updates the runtime data with user preferences such as user name, office targets, and selected days.
	/// Logs progress and status messages during execution.
	/// Throws an exception if user settings could not be retrieved.
	/// </summary>
	public async Task SetRuntimeDataAsync()
	{
		if (_runtimeDataInitialized)
		{
			logService.Debug("Runtime data already set. Skipping.");
			return;
		}

		var userSettings = await databaseService.GetUserSettingAsync();
		if (userSettings is null)
			throw new Exception("User settings could not be retrieved.");

		RuntimeData.UserName = userSettings.UserName;
		RuntimeData.HomeOfficeTargetQuoted = userSettings.HomeOfficeTargetQuoted;
		RuntimeData.OfficeTargetQuoted = userSettings.OfficeTargetQuoted;
		RuntimeData.HomeOfficeDays = userSettings.HomeOfficeDays;
		RuntimeData.OfficeDays = userSettings.OfficeDays;
		_runtimeDataInitialized = true;
	}

	#endregion


}
