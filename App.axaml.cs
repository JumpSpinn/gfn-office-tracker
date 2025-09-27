using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using OfficeTracker.ViewModels;
using OfficeTracker.Views.Windows;
using MainWindowViewModel = OfficeTracker.ViewModels.Windows.MainWindowViewModel;

namespace OfficeTracker;

public sealed partial class App : Application
{
    public IServiceProvider? Services { get; private set; }
    
    public override void Initialize()
        =>  AvaloniaXamlLoader.Load(this);
    
    public static List<string> StartupArgs { get; set; } = [];
    
    public static Window? MainWindow { get; set; }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        DisableAvaloniaDataAnnotationValidation();
        
        if(desktop.Args is not null)
            StartupArgs = desktop.Args.ToList();

        Services = new ServiceCollection()
            .AutoRegister()
            .BuildServiceProvider();

        var vm = Services.GetRequiredService<MainWindowViewModel>();
        MainWindow = desktop.MainWindow = new MainWindow()
        {
            DataContext = vm
        };

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
            BindingPlugins.DataValidators.Remove(plugin);
    }
}