namespace OfficeTracker.Helpers;

/// <summary>
/// Provides utility methods for interacting with the file explorer or folder navigation system on different operating systems.
/// </summary>
public static class ExplorerHelper
{
	/// <summary>
	/// Opens the given folder in the operating system's default file explorer.
	/// </summary>
	/// <param name="path">The full path of the folder to open. Must not be null or empty.</param>
	public static void OpenFolder(string path)
	{
		if (string.IsNullOrEmpty(path)) return;

		if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			Process.Start("explorer.exe", $"/select,\"{path}\"");
		else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			Process.Start("open", $"/select,\"{path}\"");
		else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			Process.Start("xdg-open", $"/select,\"{path}\"");
		else
		{
			try
			{
				using Process p = new();
				p.StartInfo = new() { FileName = path, UseShellExecute = true, };
				p.Start();
			}
			catch(Exception _)
			{
				var url = new Uri(path).AbsoluteUri;
				Process.Start(new ProcessStartInfo() { FileName = url, UseShellExecute = true });
			}
		}
	}
}
