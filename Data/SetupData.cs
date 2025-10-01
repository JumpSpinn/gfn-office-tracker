namespace OfficeTracker.Data;

public static class SetupData
{
	public const string SETUP_NAME = "Setup-Assistent";

	public static class WelcomePage
	{
		public const string DESCRIPTION = "Damit du den Office-Tracker gut nutzen kannst, benötigen wir einige Daten um Statistiken deiner Anwesenheit zu berechnen.";
		public const string HINT = "Du kannst zu jederzeit zum vorherigen Schritt zurückkehren um noch Änderungen vorzunehmen.";
		public const string RIGHT_BUTTON_TEXT = "Starten";
	}

	public static class NamePage
	{
		public const string STEP_TEXT = "Schritt 1";
		public const string DESCRIPTION = "Damit wir wissen wer du eigentlich bist, verrate uns doch wie du eigentlich heißt?";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	public static class OptionPage
	{
		public const string STEP_TEXT = "Schritt 2";
		public const string DESCRIPTION = "Teile uns mit, welche Quote du einhalten musst, damit wir dies in unseren Berechnungen mit einbeziehen können.";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	public static class DaysPage
	{
		public const string STEP_TEXT = "Schritt 3";
		public const string DESCRIPTION = "Kreuze bitte alle Wochentage an, an denen du standardmäßig im HomeOffice bist:";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	public static class DataPage
	{
		public const string STEP_TEXT = "Schritt 4";
		public const string DESCRIPTION = "Gebe bitte an wie viele Tage du bereits im HomeOffice und am Standort warst:";
		public const string CHECKBOX_TEXT = "Heutiger Tag ist mit einberechnet!";
		public const string RIGHT_BUTTON_TEXT = "Weiter";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}

	public static class CompletedPage
	{
		public const string STEP_TEXT = "Zusammenfassung";
		public const string RIGHT_BUTTON_TEXT = "Setup fertigstellen";
		public const string LEFT_BUTTON_TEXT = "Zurück";
	}
}
