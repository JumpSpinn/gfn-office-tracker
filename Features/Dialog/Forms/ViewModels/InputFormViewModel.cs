namespace OfficeTracker.Features.Dialog.Forms.ViewModels;

public sealed partial class InputFormViewModel : ViewModelBase
{
	#region DESCRIPTION

	[ObservableProperty]
	private bool _descriptionVisible;

	[ObservableProperty]
	private string _description = string.Empty;

	public void SetDescription(string description)
	{
		if (description == Description) return;
		Description = description;
		DescriptionVisible = !string.IsNullOrWhiteSpace(Description);
	}

	#endregion

	#region TITLE

	[ObservableProperty]
	private string _title = string.Empty;

	public void SetTitle(string title)
	{
		if (title == Title) return;
		Title = title;
	}

	#endregion

	#region PLACEHOLDER

	[ObservableProperty]
	private string _placeholder = string.Empty;

	public void SetPlaceholder(string placeholder)
	{
		if (placeholder == Placeholder) return;
		Placeholder = placeholder;
	}

	#endregion

	#region INPUT

	[ObservableProperty]
	private string _input = string.Empty;

	public void SetInput(string input)
	{
		if (input == Input) return;
		Input = input;
	}

	#endregion


}
