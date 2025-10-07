namespace OfficeTracker.Services.Config.Models;

/// <summary>
/// Represents local configuration settings for the application window, including position and size.
/// This class implements the INotifyPropertyChanged interface to enable property change notifications.
/// </summary>
public sealed class LocalConfig : INotifyPropertyChanged
{
	#region WINDOW POSITION

	/// <summary>
	/// Stores the current position of the application window as a ConfigIntVector object.
	/// The X and Y components represent the horizontal and vertical positions of the window on the screen, respectively.
	/// Updated when the window is repositioned and triggers a property change notification.
	/// </summary>
	private ConfigIntVector _windowPosition
		= new(MainWindowData.WindowPosition.X, MainWindowData.WindowPosition.Y);

	/// <summary>
	/// Represents the current position of the application window as a ConfigIntVector object.
	/// Allows getting or setting the horizontal (X) and vertical (Y) screen coordinates of the window.
	/// Updates the backing field and triggers a property change notification when modified.
	/// </summary>
	public ConfigIntVector WindowPosition
	{
		get => _windowPosition;
		set
		{
			if (value.X == _windowPosition.X && value.Y == _windowPosition.Y) return;
			_windowPosition = value;
			OnPropertyChanged();
		}
	}

	#endregion

	#region WINDOW SIZE

	/// <summary>
	/// Represents the dimensions of the application window as a ConfigIntVector object.
	/// The X and Y components correspond to the width and height of the window in pixels, respectively.
	/// This value is initialized based on the MainWindowData.WindowSize and updated
	/// when the window size changes, triggering a property change notification.
	/// </summary>
	private ConfigIntVector _windowSize
		= new((int)MainWindowData.WindowSize.Width, (int)MainWindowData.WindowSize.Height);

	/// <summary>
	/// Provides the size of the application window as a ConfigIntVector object.
	/// The X component represents the width of the window in pixels, and the Y component represents the height in pixels.
	/// This property is monitored for changes and triggers a property change notification when updated.
	/// </summary>
	public ConfigIntVector WindowSize
	{
		get => _windowSize;
		set
		{
			if (value.X == _windowSize.X && value.Y == _windowSize.Y) return;
			_windowSize = value;
			OnPropertyChanged();
		}
	}

	#endregion

	#region EVENT

	/// <summary>
	/// An event used to notify subscribers when a property value changes within the LocalConfig class.
	/// Triggered any time a property setter calls the OnPropertyChanged method, passing the name of the updated property.
	/// Commonly utilized for data-binding scenarios in MVVM architecture to reflect UI changes dynamically.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Notifies listeners that a property value has changed.
	/// </summary>
	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	#endregion
}
