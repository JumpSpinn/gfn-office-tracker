namespace OfficeTracker;

/// <summary>
/// ViewLocator is responsible for resolving view instances for corresponding view models.
/// It implements the <see cref="IDataTemplate"/> interface to build views dynamically.
/// </summary>
public sealed class ViewLocator : IDataTemplate
{
    private IServiceProvider? _serviceProvider;

    /// <summary>
    /// Constructs and returns a view instance corresponding to the given view model object.
    /// This method resolves the appropriate view type based on the naming convention
    /// and uses the service provider for dependency injection if available.
    /// </summary>
    public Control? Build(object? param)
    {
        if (param is null) return null;
        _serviceProvider ??= ((App)Application.Current!).Services;

        var name = param.GetType().FullName;
        if (name is null) return null;

        if (name.EndsWith("ViewModel"))
            name = name[..^"ViewModel".Length];

        name = name.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if(type is null)
            return new TextBlock { Text = "Not Found: " + name };

        Control? control = null;
        if (_serviceProvider is not null)
        {
            if (_serviceProvider.GetService(type) is Control c)
                control = c;
            else
                control = ActivatorUtilities.CreateInstance(_serviceProvider, type) as Control;
        }

        control!.DataContext = param;
        return control;
    }

    /// <summary>
    /// Determines whether the provided data object matches the expected type for a view model.
    /// This method verifies if the data object is of type <see cref="ViewModelBase"/>.
    /// </summary>
    public bool Match(object? data)
	    => data is ViewModelBase;
}
