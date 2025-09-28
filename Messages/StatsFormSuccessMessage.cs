namespace OfficeTracker.Messages;

public class StatsFormSuccessMessage(bool success) : ValueChangedMessage<bool>(success);
