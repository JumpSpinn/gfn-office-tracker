namespace OfficeTracker.Helpers;

/// <summary>
/// Provides utility methods for interacting with the file explorer or folder navigation system on different operating systems.
/// </summary>
public static class ExplorerHelper
{
	/// <summary>
	/// Opens a folder in the file explorer or equivalent system application based on the current operating system.
	/// </summary>
	/// <param name="path">The path of the folder to open. Can be a file system path or a URI. It must not be null or empty.</param>
	public static void OpenFolder(string path)
	{
		if (string.IsNullOrEmpty(path)) return;

		if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			Process.Start("explorer.exe", path);
		else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			Process.Start("open", path);
		else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			Process.Start("xdg-open", path);
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
