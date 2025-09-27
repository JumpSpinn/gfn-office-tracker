using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using FluentAvalonia.UI.Controls;

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
        await dialog.ShowAsync();
    }

    private static void FitApplicationSize(this ContentDialog dialog, double margin = 50)
    {
        dialog.Resources["ContentDialogMaxWidth"] = App.MainWindow?.Width - margin;
        dialog.Resources["ContentDialogMaxHeight"] = App.MainWindow?.Height - margin;
    }

    public static Task<ContentDialogResult> ShowAsyncCorrectly(this ContentDialog dialog)
    {
        dialog.FitApplicationSize();
        return dialog.ShowAsync();
    }
}