// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Threading.Tasks;
// using AngleSharp;
// using AngleSharp.Html.Dom;
//
// namespace YourApp.Services
// {
//     public class AttendanceRecord
//     {
//         public DateTime Date { get; set; }
//         public string HomeofficeStatus { get; set; }
//         public TimeSpan? LoginTime { get; set; }
//         public TimeSpan? LogoutTime { get; set; }
//         public TimeSpan? CorrectedLogin { get; set; }
//         public TimeSpan? CorrectedLogout { get; set; }
//         public bool IsHomeoffice => HomeofficeStatus?.Contains("🏠") == true;
//         public bool IsOffice => HomeofficeStatus?.Contains("🏢") == true;
//     }
//
//     public class AttendanceService
//     {
//         private readonly HttpClient _httpClient;
//         private readonly IConfiguration _config;
//
//         public AttendanceService(HttpClient httpClient)
//         {
//             _httpClient = httpClient;
//             _config = Configuration.Default.WithDefaultLoader();
//         }
//
//         public async Task<List<AttendanceRecord>> GetAttendanceDataAsync(string loginUrl, string username, string password)
//         {
//             try
//             {
//                 // 1. Login zur Website
//                 await LoginAsync(loginUrl, username, password);
//
//                 // 2. Zur Fehlzeitenmeldung navigieren
//                 var attendancePageUrl = "URL_ZUR_FEHLZEITENMELDUNG"; // Ersetzen Sie dies
//                 var response = await _httpClient.GetStringAsync(attendancePageUrl);
//
//                 // 3. HTML parsen
//                 var document = await BrowsingContext.New(_config).OpenAsync(req => req.Content(response));
//
//                 return ParseAttendanceData(document);
//             }
//             catch (Exception ex)
//             {
//                 throw new Exception($"Fehler beim Laden der Anwesenheitsdaten: {ex.Message}", ex);
//             }
//         }
//
//         private async Task LoginAsync(string loginUrl, string username, string password)
//         {
//             // Login-Seite laden
//             var loginPage = await _httpClient.GetStringAsync(loginUrl);
//             var loginDocument = await BrowsingContext.New(_config).OpenAsync(req => req.Content(loginPage));
//
//             // Login-Form finden
//             var form = loginDocument.QuerySelector("form") as IHtmlFormElement;
//             if (form == null) throw new Exception("Login-Form nicht gefunden");
//
//             // Form-Daten vorbereiten
//             var formData = new List<KeyValuePair<string, string>>();
//
//             foreach (var input in form.QuerySelectorAll("input"))
//             {
//                 var inputElement = input as IHtmlInputElement;
//                 if (inputElement?.Type == "text" || inputElement?.Name?.ToLower().Contains("user") == true)
//                     formData.Add(new KeyValuePair<string, string>(inputElement.Name, username));
//                 else if (inputElement?.Type == "password" || inputElement?.Name?.ToLower().Contains("pass") == true)
//                     formData.Add(new KeyValuePair<string, string>(inputElement.Name, password));
//                 else if (inputElement?.Type == "hidden")
//                     formData.Add(new KeyValuePair<string, string>(inputElement.Name, inputElement.Value));
//             }
//
//             // Login ausführen
//             var content = new FormUrlEncodedContent(formData);
//             await _httpClient.PostAsync(form.Action ?? loginUrl, content);
//         }
//
//         private List<AttendanceRecord> ParseAttendanceData(AngleSharp.Dom.IDocument document)
//         {
//             var records = new List<AttendanceRecord>();
//
//             // Tabelle finden (anpassen je nach HTML-Struktur)
//             var rows = document.QuerySelectorAll("tr").Skip(1); // Skip header
//
//             foreach (var row in rows)
//             {
//                 var cells = row.QuerySelectorAll("td");
//                 if (cells.Length >= 6)
//                 {
//                     var record = new AttendanceRecord
//                     {
//                         Date = ParseDate(cells[0].TextContent),
//                         HomeofficeStatus = cells[1].TextContent.Trim()
//                     };
//
//                     records.Add(record);
//                 }
//             }
//
//             return records;
//         }
//
//         private DateTime ParseDate(string dateText)
//         {
//             if (DateTime.TryParseExact(dateText, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
//                 return date;
//             return DateTime.MinValue;
//         }
//
//         private TimeSpan? ParseTime(string timeText)
//         {
//             if (string.IsNullOrWhiteSpace(timeText) || timeText.Contains("---"))
//                 return null;
//
//             if (TimeSpan.TryParseExact(timeText, @"hh\:mm", null, out var time))
//                 return time;
//
//             return null;
//         }
//     }
//
//     public class AttendanceSummary
//     {
//         public int HomeOfficeDays { get; set; }
//         public int OfficeDays { get; set; }
//     }
//
//     public static class AttendanceAnalyzer
//     {
//         public static AttendanceSummary AnalyzeAttendance(List<AttendanceRecord> records)
//         {
//             var validRecords = records.Where(r => r.Date != DateTime.MinValue).ToList();
//
//             var summary = new AttendanceSummary
//             {
//                 HomeOfficeDays = validRecords.Count(r => r.IsHomeoffice),
//                 OfficeDays = validRecords.Count(r => r.IsOffice)
//             };
//
//             return summary;
//         }
//     }
// }
