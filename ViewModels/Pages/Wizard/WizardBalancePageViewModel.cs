namespace OfficeTracker.ViewModels.Pages.Wizard;

/// <summary>
/// Represents the ViewModel for the Setup Balance page of the application.
/// </summary>
[RegisterSingleton]
public sealed partial class WizardBalancePageViewModel : ViewModelBase
{
	public WizardBalancePageViewModel()
	{
		HomeOfficePercentageDisplay = "50%";
		OfficePercentageDisplay = "50%";
		HomeOfficePercentage = 50;
		OfficePercentage = 50;
		SliderValue = 50;
	}

	[ObservableProperty]
	private string _homeOfficePercentageDisplay;

	[ObservableProperty]
	private string _officePercentageDisplay;

	[ObservableProperty]
	private double _sliderValue;

	public uint HomeOfficePercentage { get; private set; }
	public uint OfficePercentage { get; private set; }

	/// <summary>
	/// Updates the home office percentage, office percentage, and slider value
	/// based on the given slider value and its maximum value.
	/// </summary>
	public void CalculateBalance(double sliderValue, double sliderMaxValue)
	{
		HomeOfficePercentage = (uint)sliderValue;
		OfficePercentage = (uint)(sliderMaxValue - sliderValue);
		HomeOfficePercentageDisplay = $"{HomeOfficePercentage}%";
		OfficePercentageDisplay = $"{OfficePercentage}%";
		SliderValue = sliderValue;
	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.WIZARD_DAYS);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.WIZARD_NAME);
}
