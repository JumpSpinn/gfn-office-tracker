namespace OfficeTracker.ViewModels.Pages;

public sealed partial class MainPageViewModel
{
	[ObservableProperty]
	private uint _homeOfficeDays;

	[ObservableProperty]
	private uint _officeDays;

	[ObservableProperty]
	private bool _canAddCurrentDay;

	private bool _notifyToAddCurrentDay;
	public uint HomeOfficeQuote
		=> _mainWindowService.RuntimeData.HomeOfficeTargetQuoted;
	public uint OfficeQuote
		=> _mainWindowService.RuntimeData.OfficeTargetQuoted;

	/// <summary>
	/// Asynchronously refreshes statistical data by retrieving and updating the counts for
	/// home office days, office days, and determining whether the current day can be added
	/// based on the last recorded update.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation of refreshing statistics.
	/// </returns>
	private async Task RefreshStatisticsAsync()
	{
		var data = await _databaseService.GetDayCountsFromUserSettingsAsync();
		if (data is null)
		{
			_logService.Error("Day counts could not be retrieved.");
			return;
		}

		HomeOfficeDays = data.Value.homeOfficeCount;
		OfficeDays = data.Value.officeCount;
		_notifyToAddCurrentDay = CanAddCurrentDay;

#if DEBUG
		CanAddCurrentDay = true;
#else
	    CanAddCurrentDay = !DateTimeHelper.IsToday(data.Value.lastUpdate);
#endif

		if (_notifyToAddCurrentDay)
			DialogHelper.ShowDialogAsync($"Willkommen zurück, {_mainWindowService.RuntimeData.UserName}", "Vergiss nicht den heutigen Tag einzutragen!", DialogType.INFO);
	}

	/// <summary>
	/// Asynchronously displays a dialog to log the current day as either a home office day or office day,
	/// handles the selection, and updates the application's state accordingly.
	/// </summary>
	public async Task ShowAddCurrentDayDialogAsync()
	{
		var dayForm = new CurrentDayForm();
		var dialog = new ContentDialog()
		{
			Title = "Heutigen Tag eintragen",
			Content = dayForm,
			PrimaryButtonText = "Eintragen",
			CloseButtonText = "Abbrechen",
			DefaultButton = ContentDialogButton.Primary
		};

		uint entryResult = 0;
		var dialogResult = await dialog.ShowAsyncCorrectly();
		if (dialogResult == ContentDialogResult.Primary)
		{
			if (dayForm.SelectedDayType == DayType.NONE)
				await DialogHelper.ShowDialogAsync("Höö?", "Du hast was anderes ausgewählt als HomeOffice oder Standort?!", DialogType.ERROR);
			else if(DateTimeHelper.IsInWeekend(DateTime.Today))
				await DialogHelper.ShowDialogAsync("Wochenende", "Du hast Wochenende, genieß' es.", DialogType.QUESTION);
			else if (dayForm.SelectedDayType == DayType.HOME)
				entryResult = await _databaseService.IncreaseHomeOfficeCountAsync();
			else
				entryResult = await _databaseService.IncreaseOfficeCountAsync();

			if (entryResult > 0)
			{
				await RefreshStatisticsAsync();
				await ReCalculateWeeksAsync();
				await DialogHelper.ShowDialogAsync("Eingetragen", "Dein heutiger Tag wurde aufgenommen. Alle Statistiken wurden aktualisiert!", DialogType.SUCCESS);
			}
			else
				await DialogHelper.ShowDialogAsync("Fehler", "Eintrag konnte nicht gespeichert werden.", DialogType.ERROR);
		}
	}
}
