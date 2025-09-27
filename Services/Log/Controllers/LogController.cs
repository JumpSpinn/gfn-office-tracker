namespace OfficeTracker.Services.Log.Controllers;

[RegisterSingleton]
public sealed class LogController
{
	public bool IsDebugEnabled;

	private static readonly string _logFileName = $"{DateTime.UtcNow:yyyy-MM-dd HH-mm:ss}.log";

	public static readonly string LogDirectory =
		Path.Combine(PathHelper.AppTempPath, "logs");

	public string LogFilePath =>
		Path.Combine(LogDirectory, _logFileName);

	public readonly ObservableCollection<LogMessage> Logs = [];

	public LogController()
	{
		#if DEBUG
		IsDebugEnabled = true;
		#endif
	}

	public async Task<bool> EnsureLogFile()
	{
		if (!(Directory.Exists(LogDirectory) || Directory.CreateDirectory(LogDirectory).Exists))
			return false;
		await File.AppendAllTextAsync(LogFilePath, string.Empty);
		return true;
	}

	public void RemoveLog(Guid id) =>
		Logs.Remove(Logs.First(x => x.Id == id));

	public void ClearLogs() => Logs.Clear();

	private void LogInternal(string title, string message, LogSeverity severity)
	{
		#if DEBUG
		Console.WriteLine($"{title} - {message}");
		#endif

		if (severity == LogSeverity.Debug && !IsDebugEnabled) return;

		var logMessage = new LogMessage(title, message, severity);
		Logs.Add(logMessage);

		File.AppendAllText(LogFilePath, $"{DateTime.UtcNow} - {severity} - {title} - {message}{Environment.NewLine}");
	}

	#region INFO / WARN / ERROR / DEBUG
	public void Info(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Informational);
	}

	public void Warn(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Warning);
	}

	public void Exception(Exception ex,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal(className, $"Method: {memberName} - {ex.Message}", LogSeverity.Error);
	}

	public void Error(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Error);
	}

	public void Debug(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Debug);
	}

	public void Success(string message, [CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal(className, message, LogSeverity.Success);
	}
	#endregion
}
