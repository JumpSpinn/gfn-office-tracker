namespace OfficeTracker.Core.Helpers;

/// <summary>
/// A helper class for interacting with GitHub, providing methods to create and open new issue reports.
/// </summary>
public static class GitHubHelper
{
	/// <summary>
	/// Provides functionality to open and construct GitHub issue reports for bug tracking or error reporting
	/// related to the application.
	/// </summary>
	public static class NewIssue
	{
		/// <summary>
		/// Opens a GitHub issue report URL in the default web browser, allowing users to report bugs or issues.
		/// </summary>
		public static void OpenGitHubIssueReport(Exception? exception = null, string? message = null) =>
			Process.Start(new ProcessStartInfo
			{
				FileName = CreateExceptionIssue(exception, message), UseShellExecute = true
			});

		/// <summary>
		/// Constructs a GitHub issue report URL based on the provided exception and message,
		/// encoding the data appropriately for inclusion in the query string.
		/// </summary>
		private static string CreateExceptionIssue(Exception? exception, string? message)
		{
			var title = CreateIssueTitle(message);
			var body = CreateExceptionBody(exception, message);
			var encodedBody = Uri.EscapeDataString(body);
			return $"https://github.com/JumpSpinn/gfn-office-tracker/issues/new?title={title}&body={encodedBody}&labels=bug";
		}

		/// <summary>
		/// Creates a title for a GitHub issue report based on the provided message,
		/// defaulting to a generic application error title if no message is specified.
		/// </summary>
		private static string CreateIssueTitle(string? message)
			=> $"Bug Report: {message ?? "Application-Error"}";

		/// <summary>
		/// Constructs the body of a GitHub issue report based on the given exception and message,
		/// including reproducible steps, exception details, and environment information.
		/// </summary>
		private static string CreateExceptionBody(Exception? exception, string? message)
		{
			var body = "## Bug Description\n";

			if (!string.IsNullOrEmpty(message))
				body += $"{message}\n\n";

			body += "## Steps to Reproduce\n";
			body += "1. \n";
			body += "2. \n";
			body += "3. \n\n";

			if (exception is not null)
			{
				body += "## Exception Details\n";
				body += $"```\n";
				body += exception.ToString();
				body += $"\n```\n\n";
			}

			body += "## Enviroment\n";
			body += $"- OS: {Environment.OSVersion}\n";
			body += $"- .NET Version: {Environment.Version}\n";
			body += $"- Date: {DateTime.Now:MM/dd/yyyy HH:mm:ss}";


			return body;
		}
	}
}
