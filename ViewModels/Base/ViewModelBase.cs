namespace OfficeTracker.ViewModels.Base;

/// <summary>
/// Represents the base class for all view models in the application.
/// </summary>
public class ViewModelBase : ObservableObject
{
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

    public ViewModelBase? PreviousPage { get; set; }

    public void ChangePage(Page page) => _messenger.Send(new ChangePageMessage(page));
}
