namespace OfficeTracker.ViewModels.Base;

/// <summary>
/// Represents the base class for all view models in the application.
/// </summary>
public class ViewModelBase : ObservableObject
{
    public ViewModelBase? PreviousPage { get; set; }
}
