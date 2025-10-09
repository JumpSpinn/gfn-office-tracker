global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Linq;
global using System.IO;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Globalization;
global using System.Diagnostics;
global using System.Timers;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using Avalonia;
global using Avalonia.Data.Core.Plugins;
global using Avalonia.Data.Converters;
global using Avalonia.Interactivity;
global using Avalonia.Layout;
global using Avalonia.Controls;
global using Avalonia.Controls.Templates;
global using Avalonia.Controls.Primitives;
global using Avalonia.Controls.ApplicationLifetimes;
global using Avalonia.Markup.Xaml;
global using Avalonia.Media;
global using Avalonia.Threading;

global using FluentAvalonia.UI.Windowing;
global using FluentAvalonia.UI.Controls;

global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Messaging.Messages;
global using CommunityToolkit.Mvvm.Messaging;
global using CommunityToolkit.Mvvm.Input;

global using Material.Icons;
global using Material.Icons.Avalonia;

global using OfficeTracker.Core;
global using OfficeTracker.Core.Converter;
global using OfficeTracker.Core.Entities;
global using OfficeTracker.Core.Enums;
global using OfficeTracker.Core.Helpers;
global using OfficeTracker.Core.Messages;

global using OfficeTracker.Core.Services;
global using OfficeTracker.Core.Services.Config;
global using OfficeTracker.Core.Services.Config.Controllers;
global using OfficeTracker.Core.Services.Config.Services;
global using OfficeTracker.Core.Services.Log;
global using OfficeTracker.Core.Services.Log.Controllers;
global using OfficeTracker.Core.Services.Log.Entities;

global using OfficeTracker.Features;
global using OfficeTracker.Features.Dialog;
global using OfficeTracker.Features.Dialog.Forms;

global using OfficeTracker.Features.Pages;
global using OfficeTracker.Features.Pages.Main;
global using OfficeTracker.Features.Pages.Main.Views;
global using OfficeTracker.Features.Pages.Main.ViewModels;
global using OfficeTracker.Features.Pages.Main.Services;
global using OfficeTracker.Features.Pages.Settings;
global using OfficeTracker.Features.Pages.Settings.Views;
global using OfficeTracker.Features.Pages.Settings.ViewModel;

global using OfficeTracker.Features.Screens;
global using OfficeTracker.Features.Screens.NotFound;
global using OfficeTracker.Features.Screens.NotFound.Views;
global using OfficeTracker.Features.Screens.NotFound.ViewModels;
global using OfficeTracker.Features.Screens.Splash;
global using OfficeTracker.Features.Screens.Splash.Views;
global using OfficeTracker.Features.Screens.Splash.ViewModels;
global using OfficeTracker.Features.Screens.Wizard;
global using OfficeTracker.Features.Screens.Wizard.Views;
global using OfficeTracker.Features.Screens.Wizard.ViewModels;

global using OfficeTracker.Features.Templates;
global using OfficeTracker.Features.Templates.Base;
global using OfficeTracker.Features.Templates.Lists;

global using OfficeTracker.Features.Windows;
global using OfficeTracker.Features.Windows.Main;
global using OfficeTracker.Features.Windows.Main.Controllers;
global using OfficeTracker.Features.Windows.Main.Data;
global using OfficeTracker.Features.Windows.Main.Events;
global using OfficeTracker.Features.Windows.Main.Views;
global using OfficeTracker.Features.Windows.Main.ViewModels;

global using OfficeTracker.Infrastructure;
global using OfficeTracker.Infrastructure.Database;
global using OfficeTracker.Infrastructure.Database.Models;
global using OfficeTracker.Infrastructure.Database.Services;
global using OfficeTracker.Infrastructure.Database.Factories;
