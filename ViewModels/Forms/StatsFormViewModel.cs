namespace OfficeTracker.ViewModels.Forms;

[RegisterSingleton]
public sealed partial class StatsFormViewModel : ViewModelBase
{
	private readonly LogController _logController;
	private readonly StatsFormService _statsFormService;
	private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

	public StatsFormViewModel(LogController lc, StatsFormService sfs)
	{
		_logController = lc;
		_statsFormService = sfs;
	}

	#region CALCULATE STATS

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(CalculateCommand))]
	[NotifyPropertyChangedFor(nameof(CanCalculateProperty))]
	private bool _isCalculateButtonEnabled = true;

	[ObservableProperty]
	private string _calculateButtonText = "Statistik berechnen";

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(CalculateCommand))]
	[NotifyPropertyChangedFor(nameof(CanCalculateProperty))]
	private uint _homeOfficeDays;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(CalculateCommand))]
	[NotifyPropertyChangedFor(nameof(CanCalculateProperty))]
	private uint _officeDays;

	[ObservableProperty]
	private string _homeOfficePercentage;

	[ObservableProperty]
	private string _officePercentage;

	[ObservableProperty]
	private bool _firstCalculate;

	[ObservableProperty]
	private bool _hasBeenDayCounted;

	private bool CanCalculate()
		=> HomeOfficeDays > 0 &&
		   OfficeDays > 0 &&
		   IsCalculateButtonEnabled;

	public bool CanCalculateProperty
		=> HomeOfficeDays > 0 &&
		   OfficeDays > 0 &&
		   IsCalculateButtonEnabled;

	[RelayCommand(CanExecute = nameof(CanCalculate))]
	private async Task CalculateAsync()
	{
		IsCalculateButtonEnabled = false;
		CalculateButtonText = "Statistik wird berechnet..";

		try
		{
			var calculatedStats = new StatsControl()
			{
				HomeOfficeDays = HomeOfficeDays,
				OfficeDays = OfficeDays
			};
			HomeOfficePercentage = $"{calculatedStats.HomeOfficePercentage:F2}%";
			OfficePercentage = $"{calculatedStats.OfficePercentage:F2}%";
		}
		catch (Exception e)
		{
			_logController.Error($"Error on calculate stats. Details: {e.Message}");
		}
		finally
		{
			FirstCalculate = true;
			ResetCalculateForm();
		}
	}

	private void ResetCalculateForm()
	{
		CalculateButtonText = "Statistik berechnen";
		IsCalculateButtonEnabled = true;
	}

	#endregion

	#region COMPLETE SETUP

	[RelayCommand]
	private async Task CompleteSetupAsync()
	{
		var result = await _statsFormService.CreateGeneralDataAsync(HomeOfficeDays, OfficeDays, HasBeenDayCounted);
		_messenger.Send(result is null ? new StatsFormSuccessMessage(false) : new StatsFormSuccessMessage(true));
	}

	#endregion
}
