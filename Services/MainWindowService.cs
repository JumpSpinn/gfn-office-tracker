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
}
