namespace OfficeTracker.Helpers;

/// <summary>
/// Provides helper methods for managing and displaying dialog windows in the application.
/// </summary>
public static class DialogHelper
{
	/// <summary>
	/// Displays a dialog with a specified title and message.
	/// </summary>
	/// <param name="title">The title of the dialog to be displayed.</param>
	/// <param name="message">The message content of the dialog.</param>
	public static async Task ShowDialog(string title, string message)
    {
        StackPanel sp = new() { Orientation = Orientation.Horizontal };
        sp.Children.Add(new TextBlock(){ Text = message });
        var dialog = new ContentDialog()
        {
            Title = title,
            PrimaryButtonText = "Ok",
            DefaultButton = ContentDialogButton.Primary,
            Content = sp
        };
        await ShowAsyncCorrectly(dialog);
    }

	/// <summary>
	/// Adjusts the size of a ContentDialog to fit within the bounds of the main application window, applying a specified margin.
	/// </summary>
	/// <param name="dialog">The ContentDialog instance whose size is to be adjusted.</param>
	/// <param name="margin">The margin to be subtracted from the application window's width and height. Defaults to 50.</param>
	private static void FitApplicationSize(this ContentDialog dialog, double margin = 50)
    {
        dialog.Resources["ContentDialogMaxWidth"] = App.MainWindow?.Width - margin;
        dialog.Resources["ContentDialogMaxHeight"] = App.MainWindow?.Height - margin;
    }

	/// <summary>
	/// Displays the provided ContentDialog asynchronously while ensuring it fits within the application window size.
	/// </summary>
	/// <param name="dialog">The ContentDialog to be displayed.</param>
	public static Task<ContentDialogResult> ShowAsyncCorrectly(this ContentDialog dialog)
    {
        dialog.FitApplicationSize();
        return dialog.ShowAsync();
    }
}
