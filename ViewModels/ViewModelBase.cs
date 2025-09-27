using CommunityToolkit.Mvvm.ComponentModel;

namespace OfficeTracker.ViewModels;

public class ViewModelBase : ObservableObject
{
    public ViewModelBase? PreviousPage { get; set; }
}