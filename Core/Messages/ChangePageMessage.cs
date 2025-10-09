namespace OfficeTracker.Core.Messages;

/// <summary>
/// Represents a message used to notify about a page change in the application.
/// </summary>
public sealed class ChangePageMessage(Page page) : ValueChangedMessage<Page>(page);
