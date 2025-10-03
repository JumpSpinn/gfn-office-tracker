namespace OfficeTracker.Services;

/// <summary>
/// Controller responsible for managing interactions and initialization of the main application window.
/// Provides methods to initialize and validate the state of the main application window.
/// </summary>
[RegisterSingleton]
public sealed class MainWindowService(LogService logService)
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
	/// Updates the runtime data with the provided values, including user information,
	/// work schedule details, and target quotas for home office and office days.
	/// Marks the runtime data as initialized after setting the new values.
	/// </summary>
	public void SetRuntimeDataAsync(DbUserSettings data)
	{
		if (_runtimeDataInitialized)
		{
			logService.Debug("Runtime data already set.");
			return;
		}

		RuntimeData.UserName = data.UserName;
		RuntimeData.HomeOfficeTargetQuoted = data.HomeOfficeTargetQuoted;
		RuntimeData.OfficeTargetQuoted = data.OfficeTargetQuoted;
		RuntimeData.HomeOfficeDays = data.HomeOfficeDays;
		RuntimeData.OfficeDays = data.OfficeDays;
		_runtimeDataInitialized = true;
	}

	#endregion


}
