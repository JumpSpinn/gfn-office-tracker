namespace OfficeTracker.Features.Pages.Main.ViewModels;

/// <summary>
/// Represents the ViewModel for the main page, providing functionality to manage and interact with
/// the main page's data and behavior in an MVVM architecture.
/// </summary>
[RegisterSingleton]
public sealed partial class MainPageViewModel : ViewModelBase
{
	private readonly LogController _logController;
	private readonly DatabaseService _databaseService;
	private readonly MainWindowController _mainWindowController;
	private readonly CalculateWeekService _calculateWeekService;
	private readonly ConfigController _configController;

    public MainPageViewModel(DatabaseService ds, LogController lc, MainWindowController mws, CalculateWeekService cws, ConfigController cc)
    {
	    _logController = lc;
	    _databaseService = ds;
	    _mainWindowController = mws;
	    _calculateWeekService = cws;
	    _configController = cc;
    }

    #region INITIALIZE

    [ObservableProperty]
    private bool _isInitializing = true;

    /// <summary>
    /// Asynchronously initializes the state of the MainPageViewModel by creating a new stats control
    /// and loading plannable days data.
    /// </summary>
    public async Task InitializeAsync()
    {
	    var currentSelectedTab = _configController.ConfigEntity.SelectedTab;
	    IsInitializing = true;
	    try
	    {
		    // always refresh statistics when initializing the page
		    await RefreshStatisticsAsync();
		    await LoadTabDataAsync((TabType)currentSelectedTab);
	    }
	    catch (Exception ex)
	    {
		    _logController.Exception(ex);
	    }
	    finally
	    {
		    IsInitializing = false;
		    UpdateSelectedTabIndex(currentSelectedTab);
	    }
    }

    #endregion

    #region LOAD TAB DATA

    /// <summary>
    /// Asynchronously loads data for the currently selected tab based on the specified tab type.
    /// This method determines the selected tab and invokes the corresponding data-loading
    /// functionality for plannable days, holidays, or calculated weeks.
    /// </summary>
    private async Task LoadTabDataAsync(TabType tab)
    {
	    if (SelectedTabIndex != (int)tab) return;

	    switch (tab)
	    {
		    case TabType.PLANNABLE_DAYS:
			    await LoadPlannableDaysAsync();
			    break;
		    case TabType.HOLIDAYS:
			    await LoadHolidaysAsync();
			    break;
		    case TabType.CALCULATED_WEEKS:
			    await ReCalculateWeeksAsync();
			    break;
	    }
    }

    #endregion

    /// <summary>
    /// Validates whether a selected date is valid based on specific conditions, such as being non-null,
    /// not in the past, not a weekend, and not the current date.
    /// </summary>
    /// <param name="dt">The date to validate, or null if no date is provided.</param>
    /// <returns>
    /// A tuple containing a boolean indicating validation success, a title string for error messages,
    /// and a descriptive message. If the validation succeeds, the title and message will be empty.
    /// </returns>
    private (bool Result, string Title, string Message) IsSelectedPlannableDateValid(DateTime dt)
    {
	    if(DateTimeHelper.IsToday(dt))
		    return (false, "Ungültiges Datum", "Den heutigen Tag kannst du nicht mehr planen.");
	    else if(DateTimeHelper.IsInPast(dt))
		    return (false, "Ungültiges Datum", "Der Tag liegt in der Vergangenheit.");
	    else if(DateTimeHelper.IsInWeekend(dt))
		    return (false, "Ungültiges Datum", "Du arbeitst am Wochenende?");
	    return (true, "", "");
    }
}
