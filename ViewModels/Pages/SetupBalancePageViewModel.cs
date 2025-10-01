namespace OfficeTracker.ViewModels.Pages;

/// <summary>
/// Represents the ViewModel for the Setup Balance page of the application.
/// </summary>
[RegisterSingleton]
public sealed partial class SetupBalancePageViewModel : ViewModelBase
{
	private readonly LogController _logController;

	public SetupBalancePageViewModel(LogController lc)
	{
		_logController = lc;
		HomeOfficePercentage = "50%";
		OfficePercentage = "50%";
		SliderValue = 50;
	}

	[ObservableProperty]
	private string _homeOfficePercentage;

	[ObservableProperty]
	private string _officePercentage;

	[ObservableProperty]
	private double _sliderValue;

	/// <summary>
	/// Updates the home office percentage, office percentage, and slider value
	/// based on the given slider value and its maximum value.
	/// </summary>
	public void CalculateBalance(double sliderValue, double sliderMaxValue)
	{
		HomeOfficePercentage = $"{sliderValue}%";
		OfficePercentage = $"{sliderMaxValue - sliderValue}%";
		SliderValue = sliderValue;
	}

	[RelayCommand]
	private void NextSetupPage() => ChangePage(Page.SETUP_DAYS);

	[RelayCommand]
	private void PreviousSetupPage() => ChangePage(Page.SETUP_NAME);
}
