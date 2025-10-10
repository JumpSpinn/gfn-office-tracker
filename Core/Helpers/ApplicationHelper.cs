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
		else if(Environment.ProcessPath is not null)
			Process.Start(Environment.ProcessPath);
		Environment.Exit(0);
	}
}
