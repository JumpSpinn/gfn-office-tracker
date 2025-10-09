namespace OfficeTracker.Core.Services.Log.Controllers;

/// <summary>
/// Provides logging functionality for the application, including support for information, warnings, errors,
/// exceptions, and debug messages. This class manages log storage in a specified directory and maintains an
/// observable collection of log entries for runtime inspection.
/// </summary>
[RegisterSingleton]
public sealed class LogService
{
	private readonly bool _isDebugEnabled;

	private static readonly string _logFileName = $"{DateTime.UtcNow:yyyy-MM-dd HH-mm:ss}.log";

	private static readonly string _logDirectory =
		Path.Combine(PathHelper.AppTempPath, "logs");

	private string LogFilePath =>
		Path.Combine(_logDirectory, _logFileName);

	private readonly ObservableCollection<LogMessageModel> _logs = [];

	public LogService()
	{
#if DEBUG
		_isDebugEnabled = true;
#endif
	}

	/// <summary>
	/// Ensures that the log file exists. Creates the log file if it does not exist,
	/// and ensures the directory structure for logging is in place.
	/// </summary>
	public async Task<bool> EnsureLogFile()
	{
		if (!(Directory.Exists(_logDirectory) || Directory.CreateDirectory(_logDirectory).Exists))
			return false;
		await File.AppendAllTextAsync(LogFilePath, string.Empty);
		return true;
	}

	/// <summary>
	/// Removes a log entry from the collection based on its unique identifier.
	/// </summary>
	public void RemoveLog(Guid id) =>
		_logs.Remove(_logs.First(x => x.Id == id));

	/// <summary>
	/// Clears all log entries from the collection, effectively removing all stored log messages.
	/// </summary>
	public void ClearLogs()
		=> _logs.Clear();

	/// <summary>
	/// Logs a message with a specific title and severity to internal storage and the log file.
	/// Includes optional output to the console in debug mode.
	/// </summary>
	private void LogInternal(string title, string message, LogSeverity severity)
	{
#if DEBUG
		Console.WriteLine($"{title} - {message}");
#endif

		if (severity == LogSeverity.Debug && !_isDebugEnabled) return;

		var logMessage = new LogMessageModel(title, message, severity);
		_logs.Add(logMessage);

		File.AppendAllText(LogFilePath, $"{DateTime.UtcNow} - {severity} - {title} - {message}{Environment.NewLine}");
	}

	#region INFO / WARN / ERROR / DEBUG

	/// <summary>
	/// Logs an informational message. The method captures the caller's member name and file path to include context in the log entry.
	/// </summary>
	public void Info(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Informational);
	}

	/// <summary>
	/// Logs a warning message with the specified details, including the name of the calling member
	/// and the file from which the method was invoked. This log entry is recorded with a warning severity level.
	/// </summary>
	public void Warn(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Warning);
	}

	/// <summary>
	/// Logs an exception to internal storage and the log file. Includes details about the method
	/// and class where the exception occurred. Primarily used for recording unexpected errors.
	/// </summary>
	public void Exception(Exception ex,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal(className, $"Method: {memberName} - {ex.Message}", LogSeverity.Error);
	}

	/// <summary>
	/// Logs an error message with the specified details. This method captures contextual information
	/// about the caller, including the member name and file path, and logs the error with an "Error" severity level.
	/// </summary>
	public void Error(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Error);
	}

	/// <summary>
	/// Logs debug-level messages for the application. Used to provide detailed
	/// information valuable for diagnosing issues, debugging code, or tracing
	/// the flow of execution.
	/// </summary>
	public void Debug(string message,
		[CallerMemberName] string memberName = "",
		[CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal($"{className} - {memberName}", message, LogSeverity.Debug);
	}

	/// <summary>
	/// Logs a success message with the associated class name and writes it to internal storage and the log file.
	/// </summary>
	public void Success(string message, [CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);
		LogInternal(className, message, LogSeverity.Success);
	}
	#endregion
}
