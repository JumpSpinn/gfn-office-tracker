namespace OfficeTracker.Core.Helpers;

/// <summary>
/// A helper class that provides utility methods and properties for file system path management
/// specific to the application environment.
/// </summary>
public static class PathHelper
{
	#region TEMP PATH

	/// <summary>
	/// Represents the path to the application's temporary directory.
	/// Combines the system's temp folder with the application's friendly name to create a unique,
	/// application-specific temporary storage location.
	/// </summary>
	public static readonly string AppTempPath
		= Path.Combine(Path.GetTempPath(), AppDomain.CurrentDomain.FriendlyName);

	#endregion

	#region APP DATA PATH

	/// <summary>
	/// Represents the path to the application's data directory.
	/// Combines the system's ApplicationData folder with the application's friendly name to create a unique,
	/// application-specific data storage location.
	/// </summary>
	public static readonly string AppDataPath
		= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName);

	/// <summary>
	/// Ensures that the application data directory exists.
	/// If the directory does not exist, it attempts to create it.
	/// </summary>
	/// <returns>
	/// Returns a boolean indicating whether the application data directory exists
	/// or was successfully created.
	/// </returns>
	public static bool EnsureAppDataPathExists()
		=> Directory.Exists(AppDataPath) || Directory.CreateDirectory(AppDataPath).Exists;

	#endregion
}
