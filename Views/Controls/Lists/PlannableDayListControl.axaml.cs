namespace OfficeTracker.Views.Controls.Lists;

public class PlannableDayListControl : TemplatedControl
{
	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
	{
		_addButton = e.NameScope.Find<Button>("AddButton");
		if (_addButton is not null)
			_addButton.Click += OnAddButtonClick;

		DeleteButton = e.NameScope.Find<Button>("DeleteButton");
		if (DeleteButton is not null)
		{
			DeleteButton.Click += OnDeleteButtonClick;
			DeleteButton.IsEnabled = false;
		}

		List = e.NameScope.Find<ListBox>("List");
		if (List is not null)
			List.SelectionChanged += SelectionChanged;

		base.OnApplyTemplate(e);
	}

	#region LIST

	public ListBox? List;

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

	public static readonly StyledProperty<bool> IsAddEnabledProperty = AvaloniaProperty.Register<PlannableDayListControl, bool>(
		nameof(IsAddEnabled), defaultValue: true);

	public bool IsAddEnabled
	{
		get => GetValue(IsAddEnabledProperty);
		set => SetValue(IsAddEnabledProperty, value);
	}

	public readonly RoutedEvent<RoutedEventArgs> AddButtonClickedEvent =
		RoutedEvent.Register<PlannableDayListControl, RoutedEventArgs>(nameof(PlannableDayListControl), RoutingStrategies.Bubble);

	public event EventHandler<RoutedEventArgs> AddButtonClicked
	{
		add => AddHandler(AddButtonClickedEvent, value);
		remove => RemoveHandler(AddButtonClickedEvent, value);
	}

	private void OnAddButtonClick(object? sender, RoutedEventArgs e)
	{
		if (!IsAddEnabled) return;
		RaiseEvent(new () { RoutedEvent = AddButtonClickedEvent});
	}

	#endregion

	#region DELETE

	public Button? DeleteButton;

	public readonly RoutedEvent<RoutedEventArgs> DeleteButtonClickedEvent =
		RoutedEvent.Register<PlannableDayListControl, RoutedEventArgs>(nameof(PlannableDayListControl), RoutingStrategies.Bubble);

	public event EventHandler<RoutedEventArgs> DeleteButtonClicked
	{
		add => AddHandler(DeleteButtonClickedEvent, value);
		remove => RemoveHandler(DeleteButtonClickedEvent, value);
	}

	private void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
	{
		if (!IsAddEnabled) return;
		RaiseEvent(new () { RoutedEvent = DeleteButtonClickedEvent});
	}

	#endregion

	#region SELECTION

	public uint SelectedPlannableDayId
		=> List?.SelectedItem is DbPlannableDay selectedDay ? selectedDay.Id : 0;

	private void SelectionChanged(object? sender, SelectionChangedEventArgs e)
		=> ToggleRemoveEnable();

	private void ToggleRemoveEnable()
	{
		if (List is null || DeleteButton is null) return;
		DeleteButton.IsEnabled = List.SelectedIndex >= 0;
	}

	#endregion
}

