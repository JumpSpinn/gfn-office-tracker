namespace OfficeTracker.Core.Helpers;

public static class ApplicationHelper
{
	/// <summary>
	/// Restarts the application
	/// </summary>
	public static void Restart()
	{
		var processMainModule = Process.GetCurrentProcess().MainModule;
		if (processMainModule is not null)
			Process.Start(processMainModule.FileName);
		Environment.Exit(0);
	}
}
