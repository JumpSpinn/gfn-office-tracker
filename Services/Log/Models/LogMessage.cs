namespace OfficeTracker.Services.Log.Models;

/// <summary>
/// Represents a log message with a title, message, and severity level.
/// </summary>
public readonly record struct LogMessage(string Title, string Message, LogSeverity Severity = LogSeverity.Informational)
{
	public string Title { get; } = Title;
	public string Message { get; } = Message;
	public LogSeverity Severity { get; } = Severity;

	public InfoBarSeverity InfoBarSeverity
	{
		get
		{
			if (Severity == LogSeverity.Debug)
				return InfoBarSeverity.Informational;
			return (InfoBarSeverity)((int)Severity);
		}
	}

	public Guid Id { get; } = Guid.NewGuid();
}
