namespace OfficeTracker.ViewModels.Windows;

using Base;

[RegisterSingleton]
public sealed partial class MainWindowViewModel(IServiceProvider serviceProvider) : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentPage = serviceProvider.GetRequiredService<MainPageViewModel>();
}
