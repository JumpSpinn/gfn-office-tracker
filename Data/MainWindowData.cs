namespace OfficeTracker.Data;

/// <summary>
/// Provides static data related to the properties and configuration
/// of the main application window, including size and position.
/// This class is intended to define constant information relevant
/// to the application's interface setup.
/// </summary>
public static class MainWindowData
{
	/// <summary>
	/// Represents the size of the main application window in pixels.
	/// The value is defined as a static readonly field to ensure it remains constant.
	/// </summary>
	public static readonly Size WindowSize = new(450, 650);

	/// <summary>
	/// Represents the position of the main application window on the screen in pixels.
	/// The value is defined as a static readonly field to keep it constant across executions.
	/// </summary>
	public static readonly PixelPoint WindowPosition = new(0, 0);
}
