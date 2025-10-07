namespace OfficeTracker.Views.Windows;

public sealed partial class MainWindow : AppWindow, IDisposable
{
	private const int RESIZE_TIMEOUT = 500;
	private readonly System.Timers.Timer _updateTimer = new(RESIZE_TIMEOUT)
	{
		AutoReset = false
	};

    public MainWindow()
    {
        InitializeComponent();
        PositionChanged += OnPositionChanged;
        SizeChanged += OnSizeChanged;
        _updateTimer.Elapsed += (_, _) => Dispatcher.UIThread.Post(OnPositionSizeChangeComplete);
    }

    #region MENU BAR

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

    #endregion

    #region WINDOW POSITION/SIZE CHANGE

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e) =>
	    TriggerUpdateTimer();

    private void OnPositionChanged(object? sender, PixelPointEventArgs e) =>
	    TriggerUpdateTimer();

    private void TriggerUpdateTimer()
    {
	    _updateTimer.Stop();
	    _updateTimer.Start();
    }

    private void OnPositionSizeChangeComplete()
    {
	    var pos = Position;
	    var size = ClientSize;

	    App.MainWindow = this;
	    if (((App)Application.Current)?.Services is not { } serviceProvider) return;

	    serviceProvider
		    .GetRequiredService<MainWindowEvents>()
		    .WindowSizePositionChanged(pos, size);
    }

    protected override void OnClosed(EventArgs e)
    {
	    base.OnClosed(e);
	    _updateTimer.Dispose();
    }

    public void Dispose()
	    => _updateTimer.Dispose();

    #endregion
}
