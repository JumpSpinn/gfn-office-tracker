namespace OfficeTracker.Core.Helpers;

/// <summary>
/// Provides helper methods for managing and displaying dialog windows in the application.
/// </summary>
public static class DialogHelper
{
	/// <summary>
	/// Displays a dialog with the specified title, message, and optional dialog type and button text.
	/// Allows customization of the dialog's icon and layout based on dialog type.
	/// </summary>
	public static async Task<ContentDialogResult> ShowDialogAsync(string title, string message, DialogType type = DialogType.WITHOUT_ICON, string primaryButtonText = "Ok", string secondaryButtonText = "")
	{
		object container = CreateContentContainer(type);
		object dialogContent = CreateContentWithIcon(container, type, message);

		var dialog = new ContentDialog()
		{
			Title = title,
			PrimaryButtonText = primaryButtonText,
			DefaultButton = ContentDialogButton.Close,
			IsSecondaryButtonEnabled = !string.IsNullOrEmpty(secondaryButtonText),
			SecondaryButtonText = secondaryButtonText,
			Content = dialogContent
		};

		return await ShowAsyncCorrectly(dialog);
	}

	/// <summary>
	/// Creates and returns a container object for dialog content based on the specified dialog type.
	/// The container determines the layout structure for the dialog content.
	/// </summary>
	public static object CreateContentContainer(DialogType type)
	{
		if (type == DialogType.WITHOUT_ICON)
			return new StackPanel() { Orientation = Orientation.Vertical, Spacing = 4 };
		return new Grid()
		{
			ColumnDefinitions = new ColumnDefinitions()
			{
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
			},
			ColumnSpacing = 10
		};
	}

	/// <summary>
	/// Creates and returns a content object containing a message and an optional icon based on the specified dialog type.
	/// </summary>
	public static object CreateContentWithIcon(object container, DialogType type, string message)
	{
		TextBlock text = new()
		{
			VerticalAlignment = VerticalAlignment.Center,
			Text = message,
			TextWrapping = TextWrapping.Wrap
		};

		switch (container)
		{
			case StackPanel panel:
				panel.Children.Add(text);
				return panel;
			case Grid grid:
				{
					Border iconContainer = new() { VerticalAlignment = VerticalAlignment.Center};
					MaterialIcon icon = CreateDialogIcon(type);
					iconContainer.Child = icon;

					grid.Children.Add(iconContainer);
					Grid.SetColumn(iconContainer, 0);

					grid.Children.Add(text);
					Grid.SetColumn(text, 1);

					return grid;
				}
			default:
				return container;
		}
	}

	/// <summary>
	/// Creates a MaterialIcon instance corresponding to the specified dialog type.
	/// </summary>
	private static MaterialIcon CreateDialogIcon(DialogType type) =>
		type switch
		{
			DialogType.INFO => new MaterialIcon()
			{
				Kind = MaterialIconKind.Info, Foreground = Brushes.White, Width = Options.DIALOG_SIZE, Height = Options.DIALOG_SIZE
			},
			DialogType.WARNING => new MaterialIcon()
			{
				Kind = MaterialIconKind.Warning, Foreground = Brushes.White, Width = Options.DIALOG_SIZE, Height = Options.DIALOG_SIZE
			},
			DialogType.ERROR => new MaterialIcon()
			{
				Kind = MaterialIconKind.Error, Foreground = Brushes.White, Width = Options.DIALOG_SIZE, Height = Options.DIALOG_SIZE
			},
			DialogType.QUESTION => new MaterialIcon()
			{
				Kind = MaterialIconKind.QuestionMark, Foreground = Brushes.White, Width = Options.DIALOG_SIZE, Height = Options.DIALOG_SIZE
			},
			DialogType.SUCCESS => new MaterialIcon()
			{
				Kind = MaterialIconKind.Success, Foreground = Brushes.White, Width = Options.DIALOG_SIZE, Height = Options.DIALOG_SIZE
			},
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};

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
