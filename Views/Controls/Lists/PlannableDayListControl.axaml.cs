namespace OfficeTracker.Views.Controls.Lists;

public class PlannableDayListControl : TemplatedControl
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

	public static readonly StyledProperty<ObservableCollection<DbPlannableDay>> ItemsProperty =
		AvaloniaProperty.Register<PlannableDayListControl, ObservableCollection<DbPlannableDay>>(nameof(Items), defaultValue: []);

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

	private readonly RoutedEvent<RoutedEventArgs> _addButtonClickedEvent =
		RoutedEvent.Register<PlannableDayListControl, RoutedEventArgs>(nameof(PlannableDayListControl), RoutingStrategies.Bubble);

	public event EventHandler<RoutedEventArgs> AddButtonClicked
	{
		add => AddHandler(_addButtonClickedEvent, value);
		remove => RemoveHandler(_addButtonClickedEvent, value);
	}

	private void OnAddButtonClick(object? sender, RoutedEventArgs e)
		=> RaiseEvent(new () { RoutedEvent = _addButtonClickedEvent});

	#endregion

	#region DELETE

	private Button? _deleteButton;

	private readonly RoutedEvent<RoutedEventArgs> _deleteButtonClickedEvent =
		RoutedEvent.Register<PlannableDayListControl, RoutedEventArgs>(nameof(PlannableDayListControl), RoutingStrategies.Bubble);

	public event EventHandler<RoutedEventArgs> DeleteButtonClicked
	{
		add => AddHandler(_deleteButtonClickedEvent, value);
		remove => RemoveHandler(_deleteButtonClickedEvent, value);
	}

	private void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
		=> RaiseEvent(new () { RoutedEvent = _deleteButtonClickedEvent});

	#endregion

	#region SELECTION

	public uint SelectedPlannableDayId
		=> _list?.SelectedItem is DbPlannableDay selectedDay ? selectedDay.Id : 0;

	private void SelectionChanged(object? sender, SelectionChangedEventArgs e)
		=> ToggleRemoveEnable();

	private void ToggleRemoveEnable()
	{
		if (_list is null || _deleteButton is null) return;

		_deleteButton.IsVisible = _list.Items.Count > 0;
		_deleteButton.IsEnabled = _list.SelectedIndex >= 0;
	}

	#endregion
}

