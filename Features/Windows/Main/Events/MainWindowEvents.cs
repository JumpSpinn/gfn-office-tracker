namespace OfficeTracker.Features.Windows.Main.Events;

/// <summary>
/// Provides event handling for the main window, enabling subscribers to react to changes in
/// the window's position and size.
/// </summary>
[RegisterSingleton]
public class MainWindowEvents
{
	#region SIZE POSITION CHANGE EVENT

	public delegate Task WindowSizePositionDelegate(PixelPoint position, Size size);
	public event WindowSizePositionDelegate? OnWindowSizePositionChanged;
	public void WindowSizePositionChanged(PixelPoint position, Size size)
		=> OnWindowSizePositionChanged?.Invoke(position, size);

	#endregion

	#region STARTED EVENT

	public delegate Task StartedDelegate();
	public event StartedDelegate? OnStarted;
	public void Started()
		=> OnStarted?.Invoke();

	#endregion

}
