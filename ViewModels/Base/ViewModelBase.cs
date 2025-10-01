namespace OfficeTracker.ViewModels.Base;

/// <summary>
/// Represents the base class for all view models in the application.
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
	/// <summary>
	/// Provides access to the messaging instance used for communication between components
	/// in the application, such as sending and receiving messages.
	/// </summary>
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

    public ViewModelBase? PreviousPage { get; set; }

    /// <summary>
    /// Changes the current application page to the specified <paramref name="page"/>.
    /// </summary>
    protected void ChangePage(Page page) => _messenger.Send(new ChangePageMessage(page));
}
