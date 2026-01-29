namespace OfficeTracker.Dialogs.ViewModels;

/// <summary>
/// Represents a view model for a message box.
/// </summary>
[RegisterTransient]
public sealed partial class MessageBoxViewModel : BaseMessageBox
{
	public InfoBarSeverity Severity { get; set; } = InfoBarSeverity.Informational;
}
