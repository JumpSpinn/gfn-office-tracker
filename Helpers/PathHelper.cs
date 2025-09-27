namespace OfficeTracker.Helpers;

public static class PathHelper
{
	/// <summary>
	/// Path to the temporary directory of the app
	/// </summary>
	public static readonly string AppTempPath =
		Path.Combine(Path.GetTempPath(), AppDomain.CurrentDomain.FriendlyName);

	/// <summary>
	/// Path for storing the update and backup exe
	/// </summary>
	public static readonly string UpdatePath =
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			AppDomain.CurrentDomain.FriendlyName);
}
