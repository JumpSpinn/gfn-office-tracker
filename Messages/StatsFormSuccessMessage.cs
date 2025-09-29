namespace OfficeTracker.Messages;

/// <summary>
/// Represents a message indicating the success or failure of the Stats Form processing.
/// </summary>
public class StatsFormSuccessMessage(bool success) : ValueChangedMessage<bool>(success);
