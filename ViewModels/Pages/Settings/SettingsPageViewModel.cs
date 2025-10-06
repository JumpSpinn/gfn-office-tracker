namespace OfficeTracker.ViewModels.Pages.Settings;

[RegisterSingleton]
public sealed partial class SettingsPageViewModel : ViewModelBase
{
	public SettingsPageViewModel()
	{
		ParseEnumToCollection();
	}

	#region LANGUAGE SELECTION

	/// <summary>
	/// Represents the currently selected language in the settings page.
	/// </summary>
	[ObservableProperty]
	private Language _selectedLanguage = Language.German;

	[ObservableProperty]
	private ObservableCollection<Language> _languages = new();

	/// <summary>
	/// Populates an observable collection with all values from the Language enumeration.
	/// This method iterates through the Language enum and adds each value to the _languages collection.
	/// </summary>
	private void ParseEnumToCollection()
	{
		foreach (var lang in Enum.GetValues<Language>())
			Languages.Add(lang);
	}

	/// <summary>
	/// Updates the selected language by setting the SelectedLanguage property to the provided value.
	/// </summary>
	partial void OnSelectedLanguageChanged(Language value)
		=> SelectedLanguage = value;

	#endregion
}
