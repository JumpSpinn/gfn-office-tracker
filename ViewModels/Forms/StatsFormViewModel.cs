namespace OfficeTracker.ViewModels.Forms;

/// <summary>
/// The <see cref="StatsFormViewModel"/> class serves as the ViewModel for managing
/// the statistics form component within the OfficeTracker application.
/// It is responsible for managing the interaction logic and providing data
/// required for the corresponding view.
/// </summary>
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

	/// <summary>
	/// Determines whether the calculation can be executed based on the current state.
	/// </summary>
	private bool CanCalculate()
		=> HomeOfficeDays > 0 &&
		   OfficeDays > 0 &&
		   IsCalculateButtonEnabled;

	/// <summary>
	/// Represents a read-only property that determines whether the "Calculate" action
	/// can be performed. This property returns <c>true</c> when specific conditions
	/// related to the state of the ViewModel are met.
	/// </summary>
	public bool CanCalculateProperty
		=> HomeOfficeDays > 0 &&
		   OfficeDays > 0 &&
		   IsCalculateButtonEnabled;

	/// <summary>
	/// Asynchronously calculates and updates the statistics based on the user-provided data.
	/// </summary>
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

	/// <summary>
	/// Resets the state of the calculation form to its default values,
	/// enabling future calculations and updating the relevant properties accordingly.
	/// </summary>
	private void ResetCalculateForm()
	{
		CalculateButtonText = "Statistik berechnen";
		IsCalculateButtonEnabled = true;
	}

	#endregion

	#region COMPLETE SETUP

	/// <summary>
	/// Completes the setup process by creating general data asynchronously
	/// and sends a success or failure message through the messenger service.
	/// </summary>
	[RelayCommand]
	private async Task CompleteSetupAsync()
	{
		var result = await _statsFormService.CreateGeneralDataAsync(HomeOfficeDays, OfficeDays, HasBeenDayCounted);
		_messenger.Send(result is null ? new StatsFormSuccessMessage(false) : new StatsFormSuccessMessage(true));
	}

	#endregion
}
