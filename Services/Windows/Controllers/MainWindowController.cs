namespace OfficeTracker.Services.Windows.Controllers;

/// <summary>
/// Controller responsible for managing interactions and initialization of the main application window.
/// Provides methods to initialize and validate the state of the main application window.
/// </summary>
[RegisterSingleton]
public sealed class MainWindowController
{
	private bool _initialized;
	private readonly LogController _logController;
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

	public MainWindowController(LogController lc)
	{
		_logController = lc;
	}

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

			//App.MainWindow.Position = new(_configController.Config.WindowPosition.X, _configController.Config.WindowPosition.Y);
			//App.MainWindow.Width = _configController.Config.WindowSize.X;
			//App.MainWindow.Height = _configController.Config.WindowSize.Y;

			_initialized = true;
			return true;
		}
		catch (Exception ex)
		{
			_logController.Exception(ex);
			return false;
		}
	}

	/// <summary>
	/// Changes the current application page to the specified page.
	/// Logs the page change process and sends a notification to update
	/// subscribers about the page change.
	/// </summary>
	public void ChangePage(Page page)
	{
		_logController.Debug($"Changing page to {page}");
		_messenger.Send(new ChangePageMessage(page));
	}

	/// <summary>
	/// Handles the logic for updating the application's state when the main window's
	/// position or size changes. Logs the updated position and size for debugging purposes.
	/// If the application is initialized, this method may trigger saving the updated window
	/// properties to the configuration.
	/// </summary>
	private async Task OnPositionSizeChanged(PixelPoint position, Size size)
	{
		_logController.Debug($"Window Position: X={position.X}, Y={position.Y}");
		_logController.Debug($"Window Size: Width={size.Width}, Height={size.Height}");
		if (!_initialized) return;
		// Save window properties to config
		//_configController.Config.WindowPosition = new(position.X, position.Y);
		//_configController.Config.WindowSize = new((int)size.Width, (int)size.Height);
		//await _configController.SaveConfigToFile();
	}
}
