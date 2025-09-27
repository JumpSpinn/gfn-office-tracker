namespace OfficeTracker.Services.Windows.Controllers;

[RegisterSingleton]
public sealed class MainWindowController
{
	private bool _initialized;
	private readonly LogController _logController;

	public MainWindowController(LogController lc)
	{
		_logController = lc;
	}

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
