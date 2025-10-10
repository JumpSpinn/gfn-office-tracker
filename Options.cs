namespace OfficeTracker;

/// <summary>
/// Provides configurations and constants used throughout the OfficeTracker application.
/// </summary>
public static class Options
{
	/// <summary>
	/// Contains nested configurations utilized in the OfficeTracker application.
	/// It provides specific settings related to JSON serialization options.
	/// </summary>
	public static class Config
	{
		/// <summary>
		/// Represents the JSON serialization options for the OfficeTracker application.
		/// Configures various settings such as case insensitivity of property names,
		/// inclusion of fields, handling of numeric values, and default behaviors for
		/// serialization and formatting.
		/// </summary>
		public static readonly JsonSerializerOptions JsonSerializerOptions = new()
		{
			PropertyNameCaseInsensitive = true,
			IncludeFields = true,
			NumberHandling = JsonNumberHandling.AllowReadingFromString,
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};
	}

	public const string VERSION = "0.0.2";

	public const string DB_NAME = "office-tracker.db";

    public const double DIALOG_SIZE = 40;

    public const uint CALCULATE_WEEKS_COUNT = 3;
}
