namespace OfficeTracker.ViewModels.Pages.Wizard;

[RegisterSingleton]
public sealed partial class WizardDaysPageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public WizardDaysPageViewModel(LogController lc)
	{
		_logController = lc;
	}

	[ObservableProperty]
	private bool _mondaySelected;

	[ObservableProperty]
	private bool _tuesdaySelected;

	[ObservableProperty]
	private bool _wednesdaySelected;

	[ObservableProperty]
	private bool _thursdaySelected;

	[ObservableProperty]
	private bool _fridaySelected;

	[ObservableProperty]
	private bool _saturdaySelected;

	public string SelectedDays { get; private set; } = string.Empty;

	private void ConvertSelectedBooleanDays()
	{
		var days = new[]
		{
			(MondaySelected, "Mo"),
			(TuesdaySelected, "Di"),
			(WednesdaySelected, "Mi"),
			(ThursdaySelected, "Do"),
			(FridaySelected, "Fr"),
			(SaturdaySelected, "Sa")
		};
		SelectedDays = string.Join(", ", days
			.Where(d => d.Item1)
			.Select(d => d.Item2));
	}

	[RelayCommand]
	private void NextSetupPage()
	{
		ConvertSelectedBooleanDays();
		ChangePage(Page.WIZARD_DATA);
		_logController.Debug($"Monday: {MondaySelected}, Monday: {TuesdaySelected}, Monday: {WednesdaySelected}, Monday: {ThursdaySelected}, Monday: {FridaySelected}, Monday: {SaturdaySelected}");
	}

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_BALANCE);
}
