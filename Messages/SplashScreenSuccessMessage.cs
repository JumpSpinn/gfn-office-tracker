namespace OfficeTracker.Messages;

/// <summary>
/// Represents a message indicating the success or failure of the splash screen procedures.
/// </summary>
public class SplashScreenSuccessMessage(bool success) : ValueChangedMessage<bool>(success);
