namespace OfficeTracker.Dialogs.Controllers;

/// <summary>
/// Provides methods to display message boxes with different types of messages such as error, success, warning, informational, and exception details.
/// </summary>
[RegisterSingleton]
public sealed class MessageBoxController(IServiceProvider serviceProvider, LogController logController)
{
	/// <summary>
	/// Displays an error message in a message box with a title and severity level indicating an error.
	/// </summary>
	public async Task ShowErrorAsync(string message)
		=> await ShowMessageBoxAsync("Fehler", CreateMessageBoxViewModel(message, InfoBarSeverity.Error), "OK");

	/// <summary>
	/// Displays a success message in a message box with a title and severity level indicating success.
	/// </summary>
	public async Task ShowSuccessAsync(string message)
		=> await ShowMessageBoxAsync("Erfolgreich", CreateMessageBoxViewModel(message, InfoBarSeverity.Success), "OK");

	/// <summary>
	/// Displays a warning message in a message box with a title and severity level indicating a warning.
	/// </summary>
	public async Task ShowWarningAsync(string message)
		=> await ShowMessageBoxAsync("Achtung", CreateMessageBoxViewModel(message, InfoBarSeverity.Warning), "OK");

	/// <summary>
	/// Displays an informational message in a message box with a title indicating the information type.
	/// </summary>
	public async Task ShowInfoAsync(string message)
		=> await ShowMessageBoxAsync("Information", CreateMessageBoxViewModel(message, InfoBarSeverity.Informational), "OK");

	/// <summary>
	/// Displays an exception message in a dialog, providing details of the exception and additional context if available.
	/// </summary>
	public async Task ShowExceptionAsync(Exception ex) =>
		await ShowExceptionAsync(ex, string.Empty);

	/// <summary>
	/// Displays an exception message dialog with detailed exception information and an optional additional message.
	/// </summary>
	public async Task ShowExceptionAsync(Exception ex, string additionalMessage)
	{
		if (App.MainWindow is null)
		{
			logController.Error("MainWindow is null. Dialog could not be shown.");
			return;
		}

		var message = "Exception Description";
		if (!string.IsNullOrEmpty(additionalMessage))
			message += $": \n{additionalMessage}";

		var viewModel = new ExceptionViewModel
		{
			Message = message,
			Exception = ex
		};
		var content = new ExceptionDialog{ DataContext = viewModel };
		var dialog = new ContentDialog()
		{
			Title = "Kritischer Fehler!",
			Content = content,
			PrimaryButtonText = "OK",
			DefaultButton = ContentDialogButton.Primary,
			SecondaryButtonText = "Report",
			IsSecondaryButtonEnabled = true
		};

		var result = await dialog.ShowAsync(App.MainWindow);
		if (result == ContentDialogResult.Secondary)
			GitHubHelper.NewIssue.OpenGitHubIssueReport(ex, message);
	}

	/// <summary>
	/// Displays a message box with a specified title, view model, primary button text,
	/// and optionally a secondary button text. Returns the result of the dialog interaction.
	/// </summary>
	private async Task<ContentDialogResult> ShowMessageBoxAsync(string title, MessageBoxViewModel viewModel, string primaryButtonText, string? secondaryButtonText = null)
	{
		if (App.MainWindow is null)
		{
			logController.Error("MainWindow is null. Dialog could not be shown.");
			return ContentDialogResult.None;
		}

		var dialog = new ContentDialog()
		{
			Title = title,
			Content = viewModel,
			PrimaryButtonText = primaryButtonText,
			DefaultButton = ContentDialogButton.Primary,
			IsSecondaryButtonEnabled = secondaryButtonText is not null
		};

		return await dialog.ShowAsync(App.MainWindow);
	}

	/// <summary>
	/// Creates and initializes a message box view model with a specified message and severity level.
	/// </summary>
	private MessageBoxViewModel CreateMessageBoxViewModel(string message, InfoBarSeverity severity)
	{
		var vm = ActivatorUtilities.CreateInstance<MessageBoxViewModel>(serviceProvider);
		vm.Message = message;
		vm.Severity = severity;
		return vm;
	}
}
