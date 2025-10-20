namespace OfficeTracker.Features.Windows.Main.Controllers;

/// <summary>
/// Controller responsible for managing interactions and initialization of the main application window.
/// Provides methods to initialize and validate the state of the main application window.
/// </summary>
[RegisterSingleton]
public sealed class MainWindowController
{
	private readonly LogController _logController;
	private readonly ConfigController _configController;
	private readonly MainWindowEvents _mainWindowEvents;

	public MainWindowController(LogController ls, ConfigController cc, MainWindowEvents mwe)
	{
		_logController = ls;
		_configController = cc;
		_mainWindowEvents = mwe;
		_mainWindowEvents.OnWindowSizePositionChanged += OnPositionSizeChanged;
	}

	#region INITIALIZATION

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
				_logController.Error("MainWindow not available yet. Can't initialize controller.");
				return false;
			}

			if (!App.MainWindow.IsLoaded)
			{
				_logController.Error("MainWindow available, but not loaded yet. Can't initialize controller.");
				return false;
			}

			App.MainWindow.Position = new(_configController.ConfigEntity.WindowPosition.X, _configController.ConfigEntity.WindowPosition.Y);
			App.MainWindow.Width = _configController.ConfigEntity.WindowSize.X;
			App.MainWindow.Height = _configController.ConfigEntity.WindowSize.Y;

			_initialized = true;
			_mainWindowEvents.Started();
			return true;
		}
		catch (Exception ex)
		{
			_logController.Exception(ex);
			return false;
		}
	}

	#endregion

	#region WINDOW POSITION CHANGE

	/// <summary>
	/// Updates the application's configuration with the current window position and size when they change.
	/// Persists the updated configuration to the relevant storage.
	/// Logs the changes in window position and size for debugging purposes.
	/// Does not modify the configuration if the service is uninitialized.
	/// </summary>
	private async Task OnPositionSizeChanged(PixelPoint position, Size size)
	{
		if (!_configController.ConfigEntity.RememberWindowPositionSize) return;

		_logController.Debug($"Window position: X={position.X}, Y={position.Y}");
		_logController.Debug($"Window Size: Width={size.Width}, Height={size.Height}");

		if (!_initialized) return;

		_configController.ConfigEntity.WindowPosition = new(position.X, position.Y);
		_configController.ConfigEntity.WindowSize = new((int)size.Width, (int)size.Height);

		await _configController.SaveConfigToFile();
	}

	#endregion

	#region RUNTIME DATA

	/// <summary>
	/// Indicates whether the runtime data has been initialized.
	/// This flag is used to prevent reinitialization of runtime data in the application.
	/// </summary>
	private bool _runtimeDataInitialized;

	public RuntimeDataEntity RuntimeDataEntity { get; private set; } = new();

	/// <summary>
	/// Updates the runtime data with the provided values, including user information,
	/// work schedule details, and target quotas for home office and office days.
	/// Marks the runtime data as initialized after setting the new values.
	/// </summary>
	public void SetRuntimeDataAsync(UserSettingsModel data)
	{
		if (_runtimeDataInitialized)
		{
			_logController.Debug("Runtime data already set.");
			return;
		}

		RuntimeDataEntity.UserName = data.UserName;
		RuntimeDataEntity.HomeOfficeTargetQuoted = data.HomeOfficeTargetQuoted;
		RuntimeDataEntity.OfficeTargetQuoted = data.OfficeTargetQuoted;
		RuntimeDataEntity.HomeOfficeDays = data.HomeOfficeDays;
		RuntimeDataEntity.OfficeDays = data.OfficeDays;
		_runtimeDataInitialized = true;
	}

	#endregion
}
