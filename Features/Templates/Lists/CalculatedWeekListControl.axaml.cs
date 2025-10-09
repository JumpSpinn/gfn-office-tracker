namespace OfficeTracker.Features.Templates.Lists;

public sealed class CalculatedWeekListControl : TemplatedControl
{
	public static readonly StyledProperty<string> WeekNameProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, string>(nameof(WeekName));

	public string WeekName
	{
		get => GetValue(WeekNameProperty);
		set => SetValue(WeekNameProperty, value);
	}

	public static readonly StyledProperty<DateTime> StartWeekDateTimeProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, DateTime>(nameof(StartWeekDateTime));

	public DateTime StartWeekDateTime
	{
		get => GetValue(StartWeekDateTimeProperty);
		set => SetValue(StartWeekDateTimeProperty, value);
	}

	public static readonly StyledProperty<DateTime> EndWeekDateTimeProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, DateTime>(nameof(EndWeekDateTime));

	public DateTime EndWeekDateTime
	{
		get => GetValue(EndWeekDateTimeProperty);
		set => SetValue(EndWeekDateTimeProperty, value);
	}

	public static readonly StyledProperty<uint> HomeOfficeDaysProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, uint>(nameof(HomeOfficeDays));

	public uint HomeOfficeDays
	{
		get => GetValue(HomeOfficeDaysProperty);
		set => SetValue(HomeOfficeDaysProperty, value);
	}

	public static readonly StyledProperty<uint> HomeOfficeTargetQuotedProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, uint>(nameof(HomeOfficeTargetQuoted));

	public uint HomeOfficeTargetQuoted
	{
		get => GetValue(HomeOfficeTargetQuotedProperty);
		set => SetValue(HomeOfficeTargetQuotedProperty, value);
	}

	public static readonly StyledProperty<uint> OfficeDaysProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, uint>(nameof(OfficeDays));

	public uint OfficeDays
	{
		get => GetValue(OfficeDaysProperty);
		set => SetValue(OfficeDaysProperty, value);
	}

	public static readonly StyledProperty<uint> OfficeTargetQuotedProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, uint>(nameof(OfficeTargetQuoted));

	public uint OfficeTargetQuoted
	{
		get => GetValue(OfficeTargetQuotedProperty);
		set => SetValue(OfficeTargetQuotedProperty, value);
	}

	public static readonly StyledProperty<WeekDayModel[]> WeekDaysProperty =
		AvaloniaProperty.Register<CalculatedWeekListControl, WeekDayModel[]>(nameof(WeekDays));

	public WeekDayModel[] WeekDays
	{
		get => GetValue(WeekDaysProperty);
		set => SetValue(WeekDaysProperty, value);
	}
}

