namespace OfficeTracker.Services;

/// <summary>
/// Provides services related to application data management, including
/// handling the application's data directory and its existence validation.
/// </summary>
[RegisterSingleton]
public sealed class AppDataService
{
	/// <summary>
	/// Gets the path to the application-specific data directory within the
	/// system's Application Data folder. This path is constructed using the
	/// current application's friendly name and provides a standardized location
	/// for storing application data.
	/// </summary>
	public static string AppDataPath
		=> Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName);

	/// Ensures that the application data path exists. If the directory does not exist, it will be created.
	/// <returns>
	/// True if the application data path exists or was successfully created; otherwise, false.
	/// </returns>
	public bool EnsureAppDataPathExists() =>
		Directory.Exists(AppDataPath) || Directory.CreateDirectory(AppDataPath).Exists;
}
