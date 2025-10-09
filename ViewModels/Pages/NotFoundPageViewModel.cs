namespace OfficeTracker.ViewModels.Pages;

/// <summary>
/// Represents the view model for the "Not Found" page in the application.
/// </summary>
/// <remarks>
/// This view model is used as a fallback for scenarios where a requested page
/// cannot be determined or is unavailable. It inherits from <see cref="ViewModelBase"/>.
/// </remarks>
[RegisterSingleton]
public sealed partial class NotFoundPageViewModel : ViewModelBase { }
