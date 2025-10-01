namespace OfficeTracker.Views.Pages;

/// <summary>
/// Represents the SetupBalancePage in the OfficeTracker application.
/// </summary>
public partial class SetupBalancePage : UserControl
{
	public SetupBalancePage()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Handles the event triggered when the slider value is changed.
	/// </summary>
	private void OnBalanceSliderChanged(object? sender, RangeBaseValueChangedEventArgs e)
	{
		if (DataContext is not SetupBalancePageViewModel sbpvw) return;
		if (sender is not Slider slider) return;
		sbpvw.CalculateBalance(slider.Value, slider.Maximum);
	}
}

