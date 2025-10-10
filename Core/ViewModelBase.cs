namespace OfficeTracker.Core;

/// <summary>
/// Represents the base class for all view models in the application.
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
	/// <summary>
	/// Provides access to the messaging instance used for communication between components
	/// in the application, such as sending and receiving messages.
	/// </summary>
	protected readonly IMessenger _messenger = WeakReferenceMessenger.Default;

    /// <summary>
    /// Changes the current application page to the specified <paramref name="page"/>.
    /// </summary>
    protected void ChangePage(Page page) => _messenger.Send(new ChangePageMessage(page));

    #region MAIN WINDOW BAR

    private bool _mainWindowBarVisible;

    /// <summary>
    /// Gets or sets a value indicating whether the main window's bar is visible.
    /// This property controls the UI visibility for the main window's bar section,
    /// which may contain navigation or menu items.
    /// </summary>
    public bool MainWindowBarVisible
    {
	    get => _mainWindowBarVisible;
	    private set => SetProperty(ref _mainWindowBarVisible, value);
    }

    /// <summary>
    /// Sets the visibility of the main window bar based on the specified <paramref name="visible"/> value.
    /// </summary>
    /// <param name="visible">A boolean value indicating whether the main window bar should be visible or hidden.</param>
    protected void SetMainWindowBarVisibility(bool visible)
    {
	    if (MainWindowBarVisible == visible) return;
	    Console.WriteLine($"Setting MainWindowBarVisible to {visible}");
	    MainWindowBarVisible = visible;
    }

    #endregion
}
