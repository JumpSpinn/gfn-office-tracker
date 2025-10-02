namespace OfficeTracker.Data;

/// <summary>
/// The <c>WizardData</c> class provides static constants used for the setup process of the Office Tracker application.
/// It defines text content and configurations for various setup pages in a structured and reusable manner.
/// </summary>
public static class WizardData
{
	/// <summary>
	/// Represents the name identifier used for the setup process in the Office Tracker application.
	/// This constant is primarily used across various setup-related functionalities to reference
	/// the setup configuration or process identifier.
	/// </summary>
	public const string WIZARD_NAME = "Setup-Assistent";

	/// <summary>
	/// The <c>WelcomePage</c> class contains static constants specific to the Welcome page
	/// of the Office Tracker setup process. It defines text prompts and button labels
	/// used to guide the user through the initial step of the setup.
	/// </summary>
	public static class WelcomePage
	{
		public const string DESCRIPTION = "Damit du den Office-Tracker gut nutzen kannst, benötigen wir einige Daten um Statistiken deiner Anwesenheit zu berechnen.";
		public const string HINT = "Du kannst zu jederzeit zum vorherigen Schritt zurückkehren um noch Änderungen vorzunehmen.";
		public const string RIGHT_BUTTON_TEXT = "Starten";
	}

	/// <summary>
	/// The <c>NamePage</c> class contains static constants specific to the Name page
	/// of the Office Tracker setup process. It defines text content and button labels
	/// designed to collect the user's name as part of the initial configuration steps.
	/// </summary>
	public static class NamePage
	{
		public const string STEP_TEXT = "Schritt 1";
		public const string DESCRIPTION = "Damit wir wissen wer du eigentlich bist, verrate uns doch wie du eigentlich heißt?";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	/// <summary>
	/// The <c>OptionPage</c> class contains static constants specific to the Option page
	/// of the Office Tracker setup process. It defines textual content and labels
	/// provided during the step where users configure quota information for inclusion in attendance calculations.
	/// </summary>
	public static class OptionPage
	{
		public const string STEP_TEXT = "Schritt 2";
		public const string DESCRIPTION = "Teile uns mit, welche Quote du einhalten musst, damit wir dies in unseren Berechnungen mit einbeziehen können.";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	/// <summary>
	/// The <c>DaysPage</c> class provides static constants specific to the "Days" page
	/// of the Office Tracker setup process. It defines text prompts, step information,
	/// and button labels to guide the user through the configuration of their weekly work schedule.
	/// </summary>
	public static class DaysPage
	{
		public const string STEP_TEXT = "Schritt 3";
		public const string DESCRIPTION = "Kreuze bitte alle Wochentage an, an denen du standardmäßig im HomeOffice bist:";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	/// <summary>
	/// The <c>DataPage</c> class contains static constants related to the "Data Page" in the setup process
	/// of the Office Tracker application. It defines content, such as descriptive text, prompts, and button labels,
	/// used to gather user input regarding prior attendance and home office data.
	/// </summary>
	public static class DataPage
	{
		public const string STEP_TEXT = "Schritt 4";
		public const string DESCRIPTION = "Gebe bitte an wie viele Tage du bereits im HomeOffice und am Standort warst:";
		public const string CHECKBOX_TEXT = "Heutiger Tag ist mit einberechnet!";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	/// <summary>
	/// The <c>CompletedPage</c> class provides static constants used to define text and configurations
	/// for the final step of the setup process in the Office Tracker application.
	/// It includes labels and button texts to guide users through the completion stage of the setup workflow.
	/// </summary>
	public static class CompletedPage
	{
		public const string STEP_TEXT = "Zusammenfassung";
		public const string RIGHT_BUTTON_TEXT = "Setup fertigstellen";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}
}
