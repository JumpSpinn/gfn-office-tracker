namespace OfficeTracker.ViewModels.Pages;

[RegisterSingleton]
public sealed partial class MainPageViewModel : ViewModelBase
{
	private readonly MainPageService _mainPageService;

    public MainPageViewModel(MainPageService mps)
    {
	    _mainPageService = mps;
    }

    public async Task<StatsControl?> CreateNewStatsControl()
    {
	    var data = await _mainPageService.GetGeneralDataAsync();
	    if (data is null) return null;

	    return new StatsControl()
	    {
		    HomeOfficeDays = data.HomeOfficeDays,
		    OfficeDays = data.OfficeDays
	    };
    }

    public async Task<uint> AddHomeOfficeDayAsync()
	    => await _mainPageService.AddHomeOfficeDayAsync();

    public async Task<uint> AddOfficeDayAsync()
	    => await _mainPageService.AddOfficeDayAsync();

    public async Task<DbPlannableDay?> CreatePlannableDayAsync(DayType type, DateTime date)
		=> await _mainPageService.CreatePlannableDayAsync(type, date);
}
