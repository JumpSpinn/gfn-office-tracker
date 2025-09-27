namespace OfficeTracker;

using ViewModels.Base;

public sealed class ViewLocator : IDataTemplate
{
    private IServiceProvider? _serviceProvider;

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

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
