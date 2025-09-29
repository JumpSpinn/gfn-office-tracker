namespace OfficeTracker.Views.Controls.Lists;

/// <summary>
/// Represents a custom control that facilitates interaction with a collection of plannable days.
/// </summary>
public class PlannableDayListControl : TemplatedControl
{
	/// <summary>
	/// Builds the visual tree for the control when the template is applied. This method locates the elements in the control's
	/// template and attaches event handlers where necessary.
	/// </summary>
	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
	{
		_addButton = e.NameScope.Find<Button>("AddButton");
		if (_addButton is not null)
			_addButton.Click += OnAddButtonClick;

		_deleteButton = e.NameScope.Find<Button>("DeleteButton");
		if (_deleteButton is not null)
		{
			_deleteButton.Click += OnDeleteButtonClick;
			_deleteButton.IsVisible = false;
		}

		_list = e.NameScope.Find<ListBox>("List");
		if (_list is not null)
			_list.SelectionChanged += SelectionChanged;

		base.OnApplyTemplate(e);
	}

	#region LIST

	private ListBox? _list;

	/// <summary>
	/// Identifies a styled property that allows getting or setting the collection of plannable days.
	/// This property is primarily used to bind a collection of `DbPlannableDay` objects to the `PlannableDayListControl`,
	/// enabling display and interaction with the list of plannable days within the control.
	/// </summary>
	public static readonly StyledProperty<ObservableCollection<DbPlannableDay>> ItemsProperty =
		AvaloniaProperty.Register<PlannableDayListControl, ObservableCollection<DbPlannableDay>>(nameof(Items), defaultValue: []);

	/// <summary>
	/// Gets or sets the collection of plannable days displayed in the control.
	/// This property is bound to the `ItemsSource` of the `ListBox` and determines the collection of items
	/// available within the `PlannableDayListControl`.
	/// </summary>
	public ObservableCollection<DbPlannableDay> Items
	{
		get => GetValue(ItemsProperty);
		set
		{
			SetValue(ItemsProperty, value);
			ToggleRemoveEnable();
		}
	}

	#endregion

	#region ADD

	private Button? _addButton;

	/// <summary>
	/// Defines a routed event that is triggered when the "Add" button is clicked within the control.
	/// This routed event is used to notify subscribers about user interactions with the "Add" button,
	/// supporting event routing strategies such as bubbling for external handling scenarios.
	/// </summary>
	private readonly RoutedEvent<RoutedEventArgs> _addButtonClickedEvent =
		RoutedEvent.Register<PlannableDayListControl, RoutedEventArgs>(nameof(PlannableDayListControl), RoutingStrategies.Bubble);

	/// <summary>
	/// An event that is triggered when the "Add" button in the `PlannableDayListControl` is clicked.
	/// This event allows external subscribers to respond to the user's intention to add a new plannable day.
	/// It uses a routed event mechanism to propagate the event through the control hierarchy.
	/// </summary>
	public event EventHandler<RoutedEventArgs> AddButtonClicked
	{
		add => AddHandler(_addButtonClickedEvent, value);
		remove => RemoveHandler(_addButtonClickedEvent, value);
	}

	/// <summary>
	/// Handles the click event of the "Add" button. This method raises the AddButtonClicked event,
	/// allowing external subscribers to respond to the action of adding a new plannable day.
	/// </summary>
	private void OnAddButtonClick(object? sender, RoutedEventArgs e)
		=> RaiseEvent(new () { RoutedEvent = _addButtonClickedEvent});

	#endregion

	#region DELETE

	private Button? _deleteButton;

	/// <summary>
	/// Represents a routed event used to signal when the delete button is clicked within the `PlannableDayListControl`.
	/// This event follows the bubbling strategy, making it possible for parent controls to handle the event
	/// as it propagates through the visual tree.
	/// </summary>
	private readonly RoutedEvent<RoutedEventArgs> _deleteButtonClickedEvent =
		RoutedEvent.Register<PlannableDayListControl, RoutedEventArgs>(nameof(PlannableDayListControl), RoutingStrategies.Bubble);

	/// <summary>
	/// An event that is triggered when the delete button is clicked within the `PlannableDayListControl`.
	/// This event is used to handle deletion-related actions for a selected plannable day.
	/// </summary>
	public event EventHandler<RoutedEventArgs> DeleteButtonClicked
	{
		add => AddHandler(_deleteButtonClickedEvent, value);
		remove => RemoveHandler(_deleteButtonClickedEvent, value);
	}

	/// <summary>
	/// Handles the click event of the delete button within the `PlannableDayListControl`.
	/// This method raises the `DeleteButtonClicked` event to notify subscribers of the action.
	/// </summary>
	private void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
		=> RaiseEvent(new () { RoutedEvent = _deleteButtonClickedEvent});

	#endregion

	#region SELECTION

	/// <summary>
	/// Gets the ID of the currently selected plannable day in the `PlannableDayListControl`.
	/// Returns the ID of the selected `DbPlannableDay` if a selection exists; otherwise, returns 0.
	/// This property is used to retrieve the identifier for further operations, such as editing or deleting the selected item.
	/// </summary>
	public uint SelectedPlannableDayId
		=> _list?.SelectedItem is DbPlannableDay selectedDay ? selectedDay.Id : 0;

	/// <summary>
	/// Handles the event triggered when the selection in the ListBox changes. This method ensures that the visual state of related
	/// buttons, such as enabling or hiding the delete button, is updated based on the current selection in the ListBox.
	/// </summary>
	private void SelectionChanged(object? sender, SelectionChangedEventArgs e)
		=> ToggleRemoveEnable();

	/// <summary>
	/// Updates the visibility and enabled state of the delete button based on the current state of the ListBox.
	/// This method checks the number of items in the ListBox and the current selection index to determine
	/// whether the delete button should be shown and/or enabled, ensuring proper UI interaction behavior.
	/// </summary>
	private void ToggleRemoveEnable()
	{
		if (_list is null || _deleteButton is null) return;

		_deleteButton.IsVisible = _list.Items.Count > 0;
		_deleteButton.IsEnabled = _list.SelectedIndex >= 0;
	}

	#endregion
}

