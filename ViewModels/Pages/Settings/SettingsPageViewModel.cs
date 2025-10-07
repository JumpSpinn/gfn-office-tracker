namespace OfficeTracker.ViewModels.Pages.Settings;

[RegisterSingleton]
public sealed partial class SettingsPageViewModel : ViewModelBase
{
	private readonly LogService _logService;

	public SettingsPageViewModel(LogService ls)
	{
		_logService = ls;
		ParseEnumToCollection();
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

	#region REMEMBER WINDOW POSITION

	[ObservableProperty]
	private bool _rememberWindowPosition;

	/// <summary>
	/// Updates the RememberWindowPosition property based on the provided value
	/// and logs the change in the window position preference.
	/// </summary>
	partial void OnRememberWindowPositionChanged(bool value)
	{
		RememberWindowPosition = value;
		_logService.Debug($"Remember window pos: {value}");
	}

	#endregion

	#region SAVE LOCATION

	private bool _saveLocationChanging;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(SaveLocationTruncate))]
	private string _saveLocation = PathHelper.GetDatabasePath();

	public string SaveLocationTruncate
		=> SaveLocation.Truncate(45, "..");

	[RelayCommand]
	private async Task ChangeSaveLocationAsync()
	{
		if (_saveLocationChanging) return;
		if(App.MainWindow is null) return;

#pragma warning disable CS0618 // Type or member is obsolete
		var dialog = new OpenFolderDialog();
#pragma warning restore CS0618 // Type or member is obsolete
		var result = await dialog.ShowAsync(App.MainWindow);
		if (string.IsNullOrEmpty(result)) return;

		try
		{
			_saveLocationChanging = true;
			// TODO: implement save location service to change location

			SaveLocation = result;

			DialogHelper.ShowDialogAsync("Speicherort", "Speicherort erfolgreich geändert.", DialogType.SUCCESS);
			_logService.Info($"Save location changed to: {result}");
		}
		catch (Exception e)
		{
			_logService.Exception(e);
			DialogHelper.ShowDialogAsync("Speicherort", "Fehler beim Ändern des Speicherorts.", DialogType.ERROR);
		}
		finally
		{
			_saveLocationChanging = false;
		}
	}

	[RelayCommand]
	private async Task ChangeSaveLocationToDefaultAsync()
	{
		if (_saveLocationChanging) return;

		var dialog = new ContentDialog()
		{
			Title = "Speicherort zurücksetzen",
			Content = "Möchtest du den Speicherort wirklich zurücksetzen?",
			PrimaryButtonText = "Ja",
			CloseButtonText = "Abbrechen",
			DefaultButton = ContentDialogButton.Close
		};

		var result = await dialog.ShowAsyncCorrectly();
		if (result != ContentDialogResult.Primary) return;

		try
		{
			_saveLocationChanging = true;
			// TODO: implement save location service to change location to default

			DialogHelper.ShowDialogAsync("Speicherort", "Speicherort wurde erfolgreich zurückgesetzt!", DialogType.SUCCESS);
			_logService.Info($"Save location changed to default path: {result}");
		}
		catch (Exception e)
		{
			_logService.Exception(e);
			DialogHelper.ShowDialogAsync("Speicherort", "Fehler beim Zurücksetzen des Speicherorts.", DialogType.ERROR);
		}
		finally
		{
			_saveLocationChanging = false;
		}
	}

	[RelayCommand]
	private void OpenSaveFolder()
		=> ExplorerHelper.OpenFolder(SaveLocation);

	#endregion
}
