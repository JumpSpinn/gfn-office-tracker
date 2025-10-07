namespace OfficeTracker.Services.MainWindow.Events;

/// <summary>
/// Provides event handling for the main window, enabling subscribers to react to changes in
/// the window's position and size.
/// </summary>
[RegisterSingleton]
public class MainWindowEvents
{
	public delegate Task WindowSizePositionDelegate(PixelPoint position, Size size);

	public event WindowSizePositionDelegate? OnWindowSizePositionChanged;

	public void WindowSizePositionChanged(PixelPoint position, Size size)
		=> OnWindowSizePositionChanged?.Invoke(position, size);
}
