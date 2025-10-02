namespace OfficeTracker.ViewModels.Base;

/// <summary>
/// Represents the base class for all view models in the application.
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
	/// <summary>
	/// Provides access to the messaging instance used for communication between components
	/// in the application, such as sending and receiving messages.
	/// </summary>
	protected readonly IMessenger _messenger = WeakReferenceMessenger.Default;

    /// <summary>
    /// Changes the current application page to the specified <paramref name="page"/>.
    /// </summary>
    protected void ChangePage(Page page) => _messenger.Send(new ChangePageMessage(page));


    /// <summary>
    /// Stores the current state of the blur effect used within the application.
    /// Typically utilized to enhance the visual presentation of modals or dialogs
    /// by applying a blur effect to the underlying content.
    /// </summary>
    private Effect? _blurEffect;

    /// <summary>
    /// Represents the current visual blur effect applied to UI components.
    /// This property can be used to add a blur effect to certain elements,
    /// often utilized for modals or overlays to visually distinguish content layers.
    /// </summary>
    public Effect? BlurEffect
    {
	    get => _blurEffect;
	    private set => SetProperty(ref _blurEffect, value);
    }

    /// <summary>
    /// Enables the blur effect by setting the BlurEffect property
    /// to a new instance with a predefined radius value, enhancing
    /// the visual presentation of modals or dialogs.
    /// </summary>
    protected void EnableBlurEffect()
    {
	    if (BlurEffect is not null) return;
	    BlurEffect = new BlurEffect() { Radius = Options.MODAL_BLUR_RADIUS };
    }

    /// <summary>
    /// Disables the blur effect by setting the BlurEffect property to null.
    /// Used typically to revert visual blurring after a modal or overlay interaction.
    /// </summary>
    protected void DisableBlurEffect()
	    => BlurEffect = null;
}
