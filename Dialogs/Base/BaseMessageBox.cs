namespace OfficeTracker.Dialogs.Base;

/// <summary>
/// Represents a base class for message box view models.
/// Provides common properties and functionality for message boxes in the application.
/// </summary>
public class BaseMessageBox : ViewModelBase
{
	public string Message { get; set; } = string.Empty;
}
