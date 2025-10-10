namespace OfficeTracker.Features.Pages.Settings.ViewModels;

using Dialog.Content;

/// <summary>
/// Represents the view model for the settings page of the OfficeTracker application.
/// </summary>
[RegisterSingleton]
public sealed partial class SettingsPageViewModel : ViewModelBase
{
	private readonly LogController _logController;
	private readonly ConfigController _configController;
	private readonly TimingController _timingController;

	public SettingsPageViewModel(LogController ls, ConfigController cc, TimingController tc)
	{
		_logController = ls;
		_configController = cc;
		_timingController = tc;

		ParseConfig();
		ParseLanguageEnumToCollection();

		_configController.ConfigEntity.PropertyChanged += (_, _) => ParseConfig();
	}

	/// <summary>
	/// Parses the current configuration and updates the corresponding properties.
	/// </summary>
	private void ParseConfig()
	{
		RememberWindowPositionSize = _configController.ConfigEntity.RememberWindowPositionSize;
		SelectedLanguage = _configController.ConfigEntity.Language;
		SaveLocation = _configController.ConfigEntity.DatabasePath;
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
		_configController.ConfigEntity.Language = value;
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
		_configController.ConfigEntity.RememberWindowPositionSize = value;
		_configController.SaveConfigToFile();
	}

	#endregion

	#region SAVE LOCATION

	private bool _saveLocationChanging;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(SaveLocationTruncate))]
	private string _saveLocation = string.Empty;

	public string SaveLocationTruncate
		=> SaveLocation.Truncate(45, "..");

	/// <summary>
	/// Open a file dialog to select a new save location.
	/// </summary>
	[RelayCommand]
	private async Task ChangeSaveLocationAsync()
	{
		if (_saveLocationChanging) return;
		if(App.MainWindow is null) return;

#pragma warning disable CS0618 // Type or member is obsolete
		var fileDialog = new OpenFolderDialog();
#pragma warning restore CS0618 // Type or member is obsolete
		var result = await fileDialog.ShowAsync(App.MainWindow);

		if (string.IsNullOrEmpty(result)) return;
		try
		{
			_saveLocationChanging = true;
			_configController.ConfigEntity.DatabasePath = result;
			_configController.SaveConfigToFile();

			// TODO: implement save location service to change location

			_logController.Info($"Save location changed to: {result}");
			await ShowRestartDialog();
		}
		catch (Exception e)
		{
			_logController.Exception(e);
			DialogHelper.ShowDialogAsync("Speicherort", "Fehler beim Ändern des Speicherorts.", DialogType.ERROR);
		}
		finally
		{
			_saveLocationChanging = false;
		}
	}

	/// <summary>
	/// Shows a dialog to notify the user that the application will be restarted.
	/// </summary>
	private async Task ShowRestartDialog()
	{
		var dialog = new ContentDialog
		{
			Title = "Erfolgreich",
			Content = "Speicherort wurde geändert. Office-Tracker wird in Kürze neugestartet, damit die Änderung wirksam wird..",
			IsPrimaryButtonEnabled = false,
			IsSecondaryButtonEnabled = false
		};

		dialog.ShowAsyncCorrectly();
		_timingController.SetTimeout("RestartApplication", ApplicationHelper.Restart, 5_000);
	}

	/// <summary>
	/// Shows a dialog to confirm the reset of the save location to the default location.
	/// </summary>
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
			_logController.Info($"Save location changed to default path: {result}");
		}
		catch (Exception e)
		{
			_logController.Exception(e);
			DialogHelper.ShowDialogAsync("Speicherort", "Fehler beim Zurücksetzen des Speicherorts.", DialogType.ERROR);
		}
		finally
		{
			_saveLocationChanging = false;
		}
	}

	/// <summary>
	/// Opens the save location folder in the file explorer.
	/// </summary>
	[RelayCommand]
	private void OpenSaveFolder()
		=> ExplorerHelper.OpenFolder(SaveLocation);

	#endregion
}
