namespace OfficeTracker.Features.Pages.Settings.Views;

public partial class SettingsPage : UserControl
{
	public SettingsPage()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Overrides the OnLoaded method to execute custom logic when the SettingsPage is fully loaded into the visual tree.
	/// </summary>
	protected override void OnLoaded(RoutedEventArgs e)
	{
		if(DataContext is SettingsPageViewModel spvm)
			spvm.InitializeAsync();

		base.OnLoaded(e);
	}
}

