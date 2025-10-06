namespace OfficeTracker.ViewModels.Pages.Settings;

[RegisterSingleton]
public sealed partial class SettingsPageViewModel : ViewModelBase
{
	public SettingsPageViewModel()
	{
		ParseEnumToCollection();

		// Remove this check after language support is implemented.
#if RELEASE
		IsLanguageSelectionEnabled = false;
#endif
	}

	#region LANGUAGE SELECTION

	[ObservableProperty]
	private Language _selectedLanguage = Language.German;

	[ObservableProperty]
	private ObservableCollection<Language> _languages = new();

	[ObservableProperty]
	private bool _isLanguageSelectionEnabled = true;

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
