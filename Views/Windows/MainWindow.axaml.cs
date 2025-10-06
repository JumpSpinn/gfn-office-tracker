namespace OfficeTracker.Views.Windows;

public sealed partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the toggle action for the settings menu in the main window.
    /// This method interacts with the MainWindowViewModel to change the display state
    /// of the settings menu.
    /// </summary>
    private void RequestToggleSettingsMenu(object? sender, RoutedEventArgs e)
    {
	    if (DataContext is not MainWindowViewModel mwvm) return;
	    mwvm.ToggleSettingsMenu();
    }

    private void RequestOpenSaveFolder(object? sender, RoutedEventArgs e)
    {
	    if (DataContext is not MainWindowViewModel mwvm) return;
	    mwvm.OpenSaveFolder();
    }
}
