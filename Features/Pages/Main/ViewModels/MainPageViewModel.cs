namespace OfficeTracker.Features.Pages.Main.ViewModels;

/// <summary>
/// Represents the ViewModel for the main page, providing functionality to manage and interact with
/// the main page's data and behavior in an MVVM architecture.
/// </summary>
[RegisterSingleton]
public sealed partial class MainPageViewModel : ViewModelBase
{
	private readonly LogService _logService;
	private readonly DatabaseService _databaseService;
	private readonly MainWindowController _mainWindowController;
	private readonly CalculateWeekService _calculateWeekService;

    public MainPageViewModel(DatabaseService ds, LogService lc, MainWindowController mws, CalculateWeekService cws)
    {
	    _logService = lc;
	    _databaseService = ds;
	    _mainWindowController = mws;
	    _calculateWeekService = cws;
    }

    /// <summary>
    /// Asynchronously initializes the state of the MainPageViewModel by creating a new stats control
    /// and loading plannable days data.
    /// </summary>
    public async Task InitializeAsync()
    {
	    await ReCalculateWeeksAsync();
	    await RefreshStatisticsAsync();
	    await LoadPlannableDaysAsync();
    }

    /// <summary>
    /// Validates whether a selected date is valid based on specific conditions, such as being non-null,
    /// not in the past, not a weekend, and not the current date.
    /// </summary>
    /// <param name="dt">The date to validate, or null if no date is provided.</param>
    /// <returns>
    /// A tuple containing a boolean indicating validation success, a title string for error messages,
    /// and a descriptive message. If the validation succeeds, the title and message will be empty.
    /// </returns>
    private (bool Result, string Title, string Message) IsSelectedDateValid(DateTime? dt)
    {
	    if(dt is null)
		    return (false, "Ungültiges Datum", "Du hast das Datum vergessen.");
	    else if(DateTimeHelper.IsToday((DateTime)dt))
		    return (false, "Ungültiges Datum", "Den heutigen Tag kannst du nicht mehr planen.");
	    else if(DateTimeHelper.IsInPast((DateTime)dt))
		    return (false, "Ungültiges Datum", "Der Tag liegt in der Vergangenheit.");
	    else if(DateTimeHelper.IsInWeekend((DateTime)dt))
		    return (false, "Ungültiges Datum", "Du arbeitst am Wochenende?");
	    return (true, "", "");
    }
}
