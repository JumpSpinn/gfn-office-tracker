namespace OfficeTracker.Helpers;

public static class DialogHelper
{
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

    private static void FitApplicationSize(this ContentDialog dialog, double margin = 50)
    {
        dialog.Resources["ContentDialogMaxWidth"] = App.MainWindow?.Width - margin;
        dialog.Resources["ContentDialogMaxHeight"] = App.MainWindow?.Height - margin;
    }

    public static Task<ContentDialogResult> ShowAsyncCorrectly(this ContentDialog dialog, Panel? panel = null)
    {
	    if (panel is not null)
	    {
		    panel.Effect = new BlurEffect()
		    {
			    Radius = Options.MODAL_BLUR_RADIUS
		    };

		    dialog.CloseButtonClick += (s, args) => DisableMainPanelBlur(panel);
		    dialog.PrimaryButtonClick += (s, args) => DisableMainPanelBlur(panel);
		    dialog.SecondaryButtonClick += (s, args) => DisableMainPanelBlur(panel);
	    }

        dialog.FitApplicationSize();
        return dialog.ShowAsync();
    }

    private static void DisableMainPanelBlur(Panel panel)
	    => panel.Effect = null;
}
