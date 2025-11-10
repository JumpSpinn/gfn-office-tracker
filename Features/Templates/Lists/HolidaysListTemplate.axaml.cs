namespace OfficeTracker.Features.Templates.Lists;

public class HolidaysListTemplate : TemplatedControl
{
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
	/// Identifies the dependency property that holds the collection of holidays to be displayed in the HolidaysListTemplate control.
	/// This property is represented as an observable collection of <see cref="HolidayModel"/> objects, enabling the UI
	/// to dynamically respond to changes in the collection.
	/// </summary>
	public static readonly StyledProperty<ObservableCollection<HolidayModel>> ItemsProperty =
		AvaloniaProperty.Register<PlannableDayListTemplate, ObservableCollection<HolidayModel>>(nameof(Items), defaultValue: []);

	/// <summary>
	/// Represents the collection of holiday models to be displayed in the HolidaysListTemplate control.
	/// This observable collection of <see cref="HolidayModel"/> allows the user interface to automatically update
	/// when elements are added, removed, or modified within the collection.
	/// </summary>
	public ObservableCollection<HolidayModel> Items
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
	/// Represents the routed event that is triggered when the "Add Button" is clicked within the HolidaysListTemplate control.
	/// This event enables external handlers to respond to the click action, following the bubbling routing strategy.
	/// </summary>
	private readonly RoutedEvent<RoutedEventArgs> _addButtonClickedEvent =
		RoutedEvent.Register<HolidaysListTemplate, RoutedEventArgs>(nameof(HolidaysListTemplate), RoutingStrategies.Bubble);

	/// <summary>
	/// Occurs when the "Add Button" is clicked within the HolidaysListTemplate control.
	/// This routed event allows external subscribers to respond to the action, utilizing the bubbling routing strategy.
	/// </summary>
	public event EventHandler<RoutedEventArgs> AddButtonClicked
	{
		add => AddHandler(_addButtonClickedEvent, value);
		remove => RemoveHandler(_addButtonClickedEvent, value);
	}

	/// <summary>
	/// Handles the event triggered when the Add button is clicked in the HolidaysListTemplate control.
	/// This method raises the AddButtonClicked routed event, notifying registered event handlers about this action.
	/// </summary>
	private void OnAddButtonClick(object? sender, RoutedEventArgs e)
		=> RaiseEvent(new () { RoutedEvent = _addButtonClickedEvent});

	#endregion

	#region DELETE

	private Button? _deleteButton;

	/// <summary>
	/// Represents the routed event that is triggered when the Delete button is clicked
	/// in the HolidaysListTemplate control. This event uses <see cref="RoutedEventArgs"/>
	/// for providing information about the event occurrence, and it follows the Bubbling
	/// routing strategy, allowing parent controls to handle it if needed.
	/// </summary>
	private readonly RoutedEvent<RoutedEventArgs> _deleteButtonClickedEvent =
		RoutedEvent.Register<HolidaysListTemplate, RoutedEventArgs>(nameof(HolidaysListTemplate), RoutingStrategies.Bubble);

	/// <summary>
	/// Represents the event triggered when the Delete button is clicked
	/// in the HolidaysListTemplate control. This event allows the
	/// handler to respond to user interactions for removing a holiday entry,
	/// and it utilizes <see cref="RoutedEventArgs"/> for event data structure.
	/// </summary>
	public event EventHandler<RoutedEventArgs> DeleteButtonClicked
	{
		add => AddHandler(_deleteButtonClickedEvent, value);
		remove => RemoveHandler(_deleteButtonClickedEvent, value);
	}

	/// <summary>
	/// Handles the event triggered when the Delete button is clicked in the HolidaysListTemplate control.
	/// This method raises the DeleteButtonClicked routed event, notifying registered event handlers
	/// about the occurrence of this action.
	/// </summary>
	private void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
		=> RaiseEvent(new () { RoutedEvent = _deleteButtonClickedEvent});

	#endregion

	#region SELECTION

	/// <summary>
	/// Gets the identifier of the currently selected holiday in the HolidaysListTemplate control.
	/// This property retrieves the <c>Id</c> of the selected <see cref="HolidayModel"/> from the associated <c>ListBox</c>.
	/// If no holiday is selected, the value defaults to 0.
	/// </summary>
	public uint SelectedHolidayId
		=> _list?.SelectedItem is HolidayModel selectedHoliday ? selectedHoliday.Id : 0;

	/// <summary>
	/// Handles the event triggered when the selection in the ListBox is changed.
	/// This method enables or disables the delete button based on the current state of the ListBox selection and its items.
	/// </summary>
	private void SelectionChanged(object? sender, SelectionChangedEventArgs e)
		=> ToggleRemoveEnable();

	/// <summary>
	/// Toggles the visibility and enabled state of the delete button based on the current state of the ListBox.
	/// The delete button will only be visible when the ListBox contains items and enabled when an item is selected.
	/// </summary>
	private void ToggleRemoveEnable()
	{
		if (_list is null || _deleteButton is null) return;

		_deleteButton.IsVisible = _list.Items.Count > 0;
		_deleteButton.IsEnabled = _list.SelectedIndex >= 0;
	}

	#endregion
}

