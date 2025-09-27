namespace OfficeTracker.Messages;

public class SplashScreenSuccessMessage(bool success) : ValueChangedMessage<bool>(success);
