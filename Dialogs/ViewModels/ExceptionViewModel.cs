namespace OfficeTracker.Dialogs.ViewModels;

/// <summary>
/// Represents the view model for handling exception-related messages displayed in a message box.
/// </summary>
[RegisterTransient]
public sealed class ExceptionViewModel : BaseMessageBox
{
	public Exception? Exception { get; init; }
	public string ExceptionDetails => Exception?.ToString() ?? string.Empty;
}
