namespace OfficeTracker.ViewModels.Pages.Settings;

/// <summary>
/// Represents the view model for the settings page of the OfficeTracker application.
/// </summary>
[RegisterSingleton]
public sealed partial class SettingsPageViewModel : ViewModelBase
{
	private readonly LogService _logService;
	private readonly ConfigController _configController;

	public SettingsPageViewModel(LogService ls, ConfigController cc)
	{
		_logService = ls;
		_configController = cc;

		ParseConfig();
		ParseLanguageEnumToCollection();

		_configController.Config.PropertyChanged += (_, _) => ParseConfig();
	}

	private void ParseConfig()
	{
		RememberWindowPositionSize = _configController.Config.RememberWindowPositionSize;
		SelectedLanguage = _configController.Config.Language;
		SaveLocation = _configController.Config.DatabasePath;
	}

	#region LANGUAGE SELECTION

	[ObservableProperty]
	private Language _selectedLanguage;

	[ObservableProperty]
	private ObservableCollection<Language> _languages = new();

	[ObservableProperty]
	private bool _isLanguageSelectionEnabled = true;

	/// <summary>
	/// Populates an observable collection with all values from the Language enumeration.
	/// This method iterates through the Language enum and adds each value to the _languages collection.
	/// </summary>
	private void ParseLanguageEnumToCollection()
	{
		foreach (var lang in Enum.GetValues<Language>())
			Languages.Add(lang);
	}

	/// <summary>
	/// Updates the selected language by setting the SelectedLanguage property to the provided value.
	/// </summary>
	partial void OnSelectedLanguageChanged(Language value)
	{
		_configController.Config.Language = value;
		_configController.SaveConfigToFile();
	}

	#endregion

	#region REMEMBER WINDOW SIZE

	[ObservableProperty]
	private bool _rememberWindowPositionSize;

	/// <summary>
	/// Handles behavior triggered when the RememberWindowPositionSize setting is changed.
	/// This method is invoked when the RememberWindowPositionSize property is updated
	/// to apply and propagate changes in the corresponding configuration.
	/// </summary>
	partial void OnRememberWindowPositionSizeChanged(bool value)
	{
		_configController.Config.RememberWindowPositionSize = value;
		_configController.SaveConfigToFile();
	}

	#endregion

	#region SAVE LOCATION

	private bool _saveLocationChanging;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(SaveLocationTruncate))]
	private string _saveLocation;

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
			_configController.Config.DatabasePath = result;
			// TODO: implement save location service to change location

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
