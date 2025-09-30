namespace OfficeTracker;

/// <summary>
/// Provides configurations and constants used throughout the OfficeTracker application.
/// </summary>
public static class Options
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public static readonly JsonSerializerOptions JsonIndentOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        IncludeFields = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static readonly DayOfWeek[] HomeOfficeDays
	    = [DayOfWeek.Monday, DayOfWeek.Friday];

    public static readonly DayOfWeek[] OfficeDays
	    = [DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday];

    public const uint MODAL_BLUR_RADIUS = 25;

    public const double DIALOG_SIZE = 40;

    public const string VERSION = "1.0.0";

    public const string DB_NAME = "office-tracker.db";
}
