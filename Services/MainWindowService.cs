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

	public RuntimeData RuntimeData { get; private set; } = new();

	public async Task SetRuntimeDataAsync()
	{
		var userSettings = await databaseService.GetUserSettingAsync();
		if (userSettings is null)
			throw new Exception("User settings could not be retrieved.");

		RuntimeData.UserName = userSettings.UserName;
		RuntimeData.HomeOfficeTargetQuoted = userSettings.HomeOfficeTargetQuoted;
		RuntimeData.OfficeTargetQuoted = userSettings.OfficeTargetQuoted;
		RuntimeData.HomeOfficeDays = userSettings.HomeOfficeDays;
		RuntimeData.OfficeDays = userSettings.OfficeDays;
		logService.Debug("Runtime data set successfully.");
	}
}
