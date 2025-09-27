using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using OfficeTracker.ViewModels.Pages;

namespace OfficeTracker.ViewModels.Windows;

[RegisterSingleton]
public sealed partial class MainWindowViewModel : ViewModelBase
{
    //private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
    
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _currentPage = serviceProvider.GetRequiredService<MainPageViewModel>();
        // _messenger.Register<MainWindowViewModel>(this, (_, _) =>
        // {
        //     CurrentPage = serviceProvider.GetRequiredService<MainPageViewModel>();
        // });
    }
}