namespace OfficeTracker.Core.Helpers;

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

		Console.WriteLine($"Path to open in explorer: {path}");

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			OpenFolderOnWindows(path);
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			Process.Start("open", $"-R \"{path}\"");
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			var directory = Path.GetDirectoryName(path);
			if (!string.IsNullOrEmpty(directory))
				Process.Start("xdg-open", $"\"{directory}\"");
		}
		else
		{
			try
			{
				using Process p = new();
				p.StartInfo = new() { FileName = path, UseShellExecute = true, };
				Console.WriteLine($"Try: {p.StartInfo.FileName}");
				p.Start();
			}
			catch
			{
				var url = new Uri(path).AbsoluteUri;
				Process.Start(new ProcessStartInfo() { FileName = url, UseShellExecute = true });
				Console.WriteLine($"Catch: {url}");
			}
		}
	}

	/// <summary>
	/// Opens the specified folder or selects the specified file within the Windows File Explorer.
	/// </summary>
	private static void OpenFolderOnWindows(string path)
	{
		bool isDirectory = Directory.Exists(path);
		bool isFile = File.Exists(path);

		if (!isDirectory && !isFile)
		{
			Console.WriteLine($"Path does not exist: {path}");
			return;
		}

		var arguments = isDirectory ? $"\"{path}\"" : $"/select,\"{path}\"";
		var startInfo = new ProcessStartInfo
		{
			FileName = "explorer.exe",
			Arguments = arguments,
			UseShellExecute = true,
		};

		Console.WriteLine($"Starting explorer with: {startInfo.FileName} {startInfo.Arguments}");

		try
		{
			var process = Process.Start(startInfo);
			process?.WaitForInputIdle(1_000);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error opening explorer: {ex.Message}");
		}
	}
}
