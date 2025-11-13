namespace OfficeTracker.Features.Pages.Settings.ViewModels;

/// <summary>
/// Represents the view model for the settings page of the OfficeTracker application.
/// </summary>
[RegisterSingleton]
public sealed partial class SettingsPageViewModel : ViewModelBase
{
	private readonly LogController _logController;
	private readonly ConfigController _configController;
	private readonly TimingController _timingController;
	private readonly DatabaseController _databaseController;
	private readonly DatabaseService _databaseService;
	private readonly MainWindowController _mainWindowController;

	public SettingsPageViewModel(LogController ls, ConfigController cc, TimingController tc, DatabaseController dbc, DatabaseService dbs, MainWindowController mwc)
	{
		_logController = ls;
		_configController = cc;
		_timingController = tc;
		_databaseController = dbc;
		_databaseService = dbs;
		_mainWindowController = mwc;

		UpdateConfig();
		ParseLanguageEnumToCollection();

		_configController.ConfigEntity.PropertyChanged += (_, _) => UpdateConfig();
	}

	#region INITIALIZE

	[ObservableProperty]
	private bool _isInitialized;

	/// <summary>
	/// Asynchronously initializes the settings page view model by retrieving
	/// and processing configuration data from the database and runtime environment.
	/// </summary>
	public async Task InitializeAsync()
	{
		IsInitialized = false;
		try
		{
			ParseHomeOfficeTargetQuoted();
			await ParseUsernameAsync();
			await ParseHomeOfficeDayCountAsync();
			await ParseOfficeDayCountAsync();
			await ParseDefaultWeekdaysAsync();
		}
		catch (Exception e)
		{
			_logController.Exception(e);
		}
		finally
		{
			IsInitialized = true;
		}
	}

	#endregion

	/// <summary>
	/// Parses the current configuration and updates the corresponding properties.
	/// </summary>
	private void UpdateConfig()
	{
		RememberWindowPositionSize = _configController.ConfigEntity.RememberWindowPositionSize;
		RememberSelectedTabIndex = _configController.ConfigEntity.RememberSelectedTabIndex;
		SelectedLanguage = _configController.ConfigEntity.Language;
		SaveLocation = _configController.ConfigEntity.DatabasePath;
	}

	#region LANGUAGE SELECTION

	[ObservableProperty]
	private Language _selectedLanguage;

	[ObservableProperty]
	private ObservableCollection<Language> _languages = new();

	[ObservableProperty]
	private bool _isLanguageSelectionEnabled;

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

	#region CHANGE HOMEOFFICE DAYS

	/// <summary>
	/// Asynchronously parses the home office day count and updates the corresponding property.
	/// </summary>
	private async Task ParseHomeOfficeDayCountAsync()
		=> HomeOfficeDayCount = await _databaseService.GetHomeOfficeDayCountAsync() ?? 0;

	[ObservableProperty]
	private uint _homeOfficeDayCount;

	/// <summary>
	/// Asynchronously updates the home office day count property based on user input.
	/// </summary>
	[RelayCommand]
	private async Task ChangeHomeOfficeDayCountAsync()
	{
		var inputFormContext = new InputFormViewModel();
		inputFormContext.SetDescription($"Bitte gebe ein neuen Wert ein, der für die HomeOffice Tage gespeichert werden soll.");
		inputFormContext.SetInput(HomeOfficeDayCount.ToString());
		inputFormContext.SetPlaceholder("0");
		inputFormContext.SetTitle("HomeOffice - Tage:");

		var content = new InputForm() { DataContext = inputFormContext };

		var dialog = new ContentDialog()
		{
			Title = "HomeOffice Tage ändern",
			Content = content,
			PrimaryButtonText = "Übernehmen",
			CloseButtonText = "Abbrechen",
			DefaultButton = ContentDialogButton.Close
		};

		if (await dialog.ShowAsyncCorrectly() != ContentDialogResult.Primary) return;
		if(string.IsNullOrWhiteSpace(inputFormContext.Input))
			await DialogHelper.ShowDialogAsync("HomeOffice Tage", "Bitte gib einen Wert ein.", DialogType.ERROR);
		else if(!uint.TryParse(inputFormContext.Input, out var homeOfficeDayCount))
			await DialogHelper.ShowDialogAsync("HomeOffice Tage", "Ungültige Eingabe.", DialogType.ERROR);
		else if(homeOfficeDayCount == HomeOfficeDayCount)
			await DialogHelper.ShowDialogAsync("HomeOffice Tage", "HomeOffice Tage sind identisch mit den aktuellen.", DialogType.ERROR);
		else
		{
			var newHomeOfficeDayCount = await _databaseService.UpdateHomeOfficeDayCountAsync(homeOfficeDayCount);
			if (newHomeOfficeDayCount is not null)
			{
				HomeOfficeDayCount = (uint)newHomeOfficeDayCount;
				await DialogHelper.ShowDialogAsync("HomeOffice Tage", "HomeOffice Tage wurden erfolgreich geändert!", DialogType.SUCCESS);
			}
		}
	}

	#endregion

	#region CHANGE OFFICE DAYS

	/// <summary>
	/// Asynchronously parses the office day count and updates the corresponding property.
	/// </summary>
	private async Task ParseOfficeDayCountAsync()
		=> OfficeDayCount = await _databaseService.GetOfficeDayCountAsync() ?? 0;

	[ObservableProperty]
	private uint _officeDayCount;

	/// <summary>
	/// Asynchronously updates the office day count property based on user input.
	/// </summary>
	[RelayCommand]
	private async Task ChangeOfficeDayCountAsync()
	{
		var inputFormContext = new InputFormViewModel();
		inputFormContext.SetDescription($"Bitte gebe ein neuen Wert ein, der für die Standort Tage gespeichert werden soll.");
		inputFormContext.SetInput(OfficeDayCount.ToString());
		inputFormContext.SetPlaceholder("0");
		inputFormContext.SetTitle("Standort - Tage:");

		var content = new InputForm() { DataContext = inputFormContext };

		var dialog = new ContentDialog()
		{
			Title = "Standort Tage ändern",
			Content = content,
			PrimaryButtonText = "Übernehmen",
			CloseButtonText = "Abbrechen",
			DefaultButton = ContentDialogButton.Close
		};

		if (await dialog.ShowAsyncCorrectly() != ContentDialogResult.Primary) return;
		if(string.IsNullOrWhiteSpace(inputFormContext.Input))
			await DialogHelper.ShowDialogAsync("Standort Tage", "Bitte gib einen Wert ein.", DialogType.ERROR);
		else if(!uint.TryParse(inputFormContext.Input, out var officeDayCount))
			await DialogHelper.ShowDialogAsync("Standort Tage", "Ungültige Eingabe.", DialogType.ERROR);
		else if(officeDayCount == OfficeDayCount)
			await DialogHelper.ShowDialogAsync("Standort Tage", "Standort Tage sind identisch mit den aktuellen.", DialogType.ERROR);
		else
		{
			var newOfficeDayCount = await _databaseService.UpdateOfficeDayCountAsync(officeDayCount);
			if (newOfficeDayCount is not null)
			{
				OfficeDayCount = (uint)newOfficeDayCount;
				await DialogHelper.ShowDialogAsync("Standort Tage", "Standort Tage wurden erfolgreich geändert!", DialogType.SUCCESS);
			}
		}
	}

	#endregion

	#region CHANGE USERNAME

	/// <summary>
	/// Asynchronously retrieves and updates the username property from the database using the DatabaseService.
	/// </summary>
	private async Task ParseUsernameAsync()
		=> Username = await _databaseService.GetUserNameAsync() ?? string.Empty;

	[ObservableProperty]
	private string _username = string.Empty;

	/// <summary>
	/// Opens a dialog to allow the user to change their username.
	/// Validates the input and updates the username if it is valid.
	/// Displays appropriate success or error messages based on the validation and update process.
	/// </summary>
	[RelayCommand]
	private async Task ChangeUsernameAsync()
	{
		var inputFormContext = new InputFormViewModel();
		inputFormContext.SetDescription($"Bitte gebe dein neuen Benutzernamen ein und drücke anschließend auf Übernehmen. Die maximale Zeichenlänge beträgt {Options.USERNAME_MAX_LENGTH} Zeichen.");
		inputFormContext.SetInput(Username);
		inputFormContext.SetPlaceholder("Max Mustermann");
		inputFormContext.SetTitle("Neuer Benutzername:");

		var content = new InputForm() { DataContext = inputFormContext };

		var dialog = new ContentDialog()
		{
			Title = "Benutzernamen ändern",
			Content = content,
			PrimaryButtonText = "Übernehmen",
			CloseButtonText = "Abbrechen",
			DefaultButton = ContentDialogButton.Close
		};

		if (await dialog.ShowAsyncCorrectly() != ContentDialogResult.Primary) return;

		var userNameInput = inputFormContext.Input;
		var validation = StringHelper.ValidateUserName(userNameInput);
		if(!validation.Result)
			await DialogHelper.ShowDialogAsync("Benutzername", validation.Message, DialogType.ERROR);
		else
		{
			var newUserName = await _databaseService.UpdateUserNameAsync(userNameInput);
			if (newUserName is not null)
			{
				Username = newUserName;
				_mainWindowController.RuntimeDataEntity.UserName = newUserName;
				await DialogHelper.ShowDialogAsync("Benutzername", "Benutzername wurde erfolgreich geändert!",
					DialogType.SUCCESS);
			}
		}
	}

	#endregion

	#region CHANGE DEFAULT HOMEOFFICE WEEKDAYS

	[ObservableProperty]
	private string _defaultHomeOfficeWeekdaysDisplay = string.Empty;

	private DayOfWeek[] _homeOfficeWeekDays = [];
	private DayOfWeek[] _officeWeekDays = [];

	/// <summary>
	/// Updates the display of default home office and office weekdays based on the provided data.
	/// </summary>
	private void UpdateDefaultHomeOfficeWeekdaysDisplay(DayOfWeek[]? homeOffice, DayOfWeek[]? office)
	{
		_homeOfficeWeekDays = homeOffice ?? [];
		_officeWeekDays = office ?? [];
		DefaultHomeOfficeWeekdaysDisplay = _homeOfficeWeekDays.ToCommaSeparatedString();
		if (DefaultHomeOfficeWeekdaysDisplay.Length <= 0)
			DefaultHomeOfficeWeekdaysDisplay = "Keine";
	}

	/// <summary>
	/// Fetches the default weekdays for home office and office, and updates the display properties accordingly.
	/// </summary>
	private async Task ParseDefaultWeekdaysAsync()
	{
		var homeOfficeWeekDays = await _databaseService.GetHomeOfficeDaysAsync();
		var officeWeekDays = await _databaseService.GetOfficeDaysAsync();
		UpdateDefaultHomeOfficeWeekdaysDisplay(homeOfficeWeekDays, officeWeekDays);
	}

	/// <summary>
	/// Opens a dialog allowing the user to update the default home office weekdays.
	/// Updates the persisted settings and applies changes to runtime data if the user confirms the modifications.
	/// </summary>
	[RelayCommand]
	private async Task ChangeDefaultHomeOfficeWeekdaysAsync()
	{
		var viewModel = new DefaultHomeOfficeDaysFormViewModel();
		viewModel.SetData(_homeOfficeWeekDays);
		var view = new DefaultHomeOfficeDaysForm() { DataContext = viewModel };

		var dialog = new ContentDialog()
		{
			Title = "Standard Wochentage ändern",
			Content = view,
			PrimaryButtonText = "Übernehmen",
			CloseButtonText = "Abbrechen",
			DefaultButton = ContentDialogButton.Close
		};

		if (await dialog.ShowAsyncCorrectly() != ContentDialogResult.Primary) return;

		var (homeOfficeDays, officeDays) = viewModel.GetData();
		if (homeOfficeDays.Length == 0 && officeDays.Length == 0)
			await DialogHelper.ShowDialogAsync("Standard Wochentage",
				"Wenn du diese Fehlermeldung siehst, hast du ein Preis gewonnen!", DialogType.ERROR);
		else if(homeOfficeDays == _homeOfficeWeekDays && officeDays == _officeWeekDays)
			await DialogHelper.ShowDialogAsync("Standard Wochentage", "Diese Wochentage sind bereits deine Standard Wochentage.", DialogType.ERROR);
		else if (!await _databaseService.UpdateHomeOfficeDaysAsync(homeOfficeDays) ||
		         !await _databaseService.UpdateOfficeDaysAsync(officeDays))
			await DialogHelper.ShowDialogAsync("Standard Wochentage", "Wochentage konnten nicht gespeichert werden.",
				DialogType.ERROR);
		else
		{
			_mainWindowController.RuntimeDataEntity.HomeOfficeDays = homeOfficeDays;
			_mainWindowController.RuntimeDataEntity.OfficeDays = officeDays;
			UpdateDefaultHomeOfficeWeekdaysDisplay(homeOfficeDays, officeDays);
			await DialogHelper.ShowDialogAsync("Standard Wochentage", "Änderungen wurden erfolgreich gespeichert.",
				DialogType.SUCCESS);
		}
	}

	#endregion

	#region CHANGE HOMEOFFICE QUOTA

	[ObservableProperty]
	private uint _homeOfficeTargetQuoted;

	/// <summary>
	/// Retrieves and assigns the value of the home office target quoted
	/// from the runtime data entity provided by the main window controller.
	/// </summary>
	private void ParseHomeOfficeTargetQuoted()
		=> HomeOfficeTargetQuoted = _mainWindowController.RuntimeDataEntity.HomeOfficeTargetQuoted;

	/// <summary>
	/// Initiates a dialog to change the target HomeOffice quota, validates the input, and updates the value in the database and runtime context if valid.
	/// </summary>
	[RelayCommand]
	private async Task ChangeHomeOfficeTargetQuotedAsync()
	{
		var inputFormContext = new InputFormViewModel();
		inputFormContext.SetDescription($"Bitte gebe deine neue gewünschte HomeOffice Ziel-Quote ein.");
		inputFormContext.SetInput(HomeOfficeTargetQuoted.ToString());
		inputFormContext.SetPlaceholder("1337");
		inputFormContext.SetTitle("Quote:");

		var content = new InputForm() { DataContext = inputFormContext };

		var dialog = new ContentDialog()
		{
			Title = "Quote (HomeOffice) ändern",
			Content = content,
			PrimaryButtonText = "Übernehmen",
			CloseButtonText = "Abbrechen",
			DefaultButton = ContentDialogButton.Close
		};

		if (await dialog.ShowAsyncCorrectly() != ContentDialogResult.Primary) return;

		var inputResult = inputFormContext.Input;
		if(string.IsNullOrWhiteSpace(inputResult))
			await DialogHelper.ShowDialogAsync("Quote (HomeOffice)", "Bitte gib einen Wert ein.", DialogType.ERROR);
		else if(!uint.TryParse(inputResult, out uint parsedResult))
			await DialogHelper.ShowDialogAsync("Quote (HomeOffice)", "Das ist ein ungültiger Wert.", DialogType.ERROR);
		else if(parsedResult == HomeOfficeTargetQuoted)
			await DialogHelper.ShowDialogAsync("Quote (HomeOffice)", "Diese Quote ist bereits deine aktuelle Ziel-Quote.", DialogType.ERROR);
		else if(!await _databaseService.UpdateHomeOfficeTargetQuoteAsync(parsedResult))
			await DialogHelper.ShowDialogAsync("Quote (HomeOffice)", "Änderung konnte nicht gespeichert werden.", DialogType.ERROR);
		else
		{
			HomeOfficeTargetQuoted = parsedResult;
			_mainWindowController.RuntimeDataEntity.HomeOfficeTargetQuoted = parsedResult;
			_mainWindowController.RuntimeDataEntity.OfficeTargetQuoted = 100 - parsedResult;
			await DialogHelper.ShowDialogAsync("Quote (HomeOffice)", "Neue Ziel-Quote wurde erfolgreich gespeichert.",
				DialogType.SUCCESS);
		}
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

	#region REMEMBER SELECTED TAB INDEX

	[ObservableProperty]
	private bool _rememberSelectedTabIndex;

	/// <summary>
	/// Handles changes to the RememberSelectedTabIndex property.
	/// Updates the configuration entity with the new value and saves the updated
	/// configuration to the configuration file.
	/// </summary>
	partial void OnRememberSelectedTabIndexChanged(bool value)
	{
		_configController.ConfigEntity.RememberSelectedTabIndex = value;
		if (!value)
			_configController.ConfigEntity.SelectedTab = 0; // Reset to default tab
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
		var newPath = await fileDialog.ShowAsync(App.MainWindow);

		if (string.IsNullOrEmpty(newPath)) return;
		try
		{
			_saveLocationChanging = true;

			if (!await _databaseController.BackupDatabaseAsync(newPath)) return;

			_configController.ConfigEntity.DatabasePath = newPath;
			await _configController.SaveConfigToFile();

			_logController.Info($"Save location changed to: {_configController.ConfigEntity.DatabasePath}");
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

		if (await dialog.ShowAsyncCorrectly() != ContentDialogResult.Primary) return;

		try
		{
			_saveLocationChanging = true;

			var newPath = PathHelper.AppDataPath;
			if (!await _databaseController.BackupDatabaseAsync(newPath)) return;

			_configController.ConfigEntity.DatabasePath = newPath;
			await _configController.SaveConfigToFile();

			_logController.Info($"Save location changed to default path: {_configController.ConfigEntity.DatabasePath}");
			await ShowRestartDialog();
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
	/// Opens the save location folder in the file explorer.
	/// </summary>
	[RelayCommand]
	private void OpenSaveFolder()
		=> ExplorerHelper.OpenFolder(SaveLocation);

	#endregion
}
