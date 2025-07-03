using System.Windows.Input;

namespace Photography_Tools.Components.Controls.UnitEntry;

public partial class UnitEntryControl : ContentView
{
    private IUnitConverter unitConverter;
    private int selectedUnitIndex;
    private double baseUnitValue, selectedUnitValue;
    private bool isInputFromUser = true;

    #region BindableProperties
    public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(
        nameof(ValueChangedCommand),
        typeof(ICommand),
        typeof(UnitEntryControl));

    public static readonly BindableProperty BaseUnitValueProperty = BindableProperty.Create(
        nameof(BaseUnitValue),
        typeof(double),
        typeof(UnitEntryControl),
        defaultValue: 0.0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (UnitEntryControl)bindable;
            control.BaseUnitValue = (double)newValue;
        });

    public static readonly BindableProperty SelectedUnitIndexProperty = BindableProperty.Create(
        nameof(SelectedUnitIndex),
        typeof(int),
        typeof(UnitEntryControl),
        defaultValue: 0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (UnitEntryControl)bindable;
            control.SelectedUnitIndex = (int)newValue;
        });
    #endregion

    public ICommand? ValueChangedCommand
    {
        get => (ICommand)GetValue(ValueChangedCommandProperty);
        set => SetValue(ValueChangedCommandProperty, value);
    }

    public IUnitConverter UnitConverter
    {
        get => unitConverter;
        set
        {
            if (value is null)
                return;

            unitConverter = value;
            UnitPicker.ItemsSource = unitConverter.Units;
            if (SelectedUnitIndex < unitConverter.Units.Length)
                UnitPicker.SelectedItem = unitConverter.Units[SelectedUnitIndex];
            else
                SelectedUnitIndex = 0;
        }
    }

    public double BaseUnitValue
    {
        get => baseUnitValue;
        set
        {
            if (baseUnitValue != value && value >= MinValueBaseUnit && value <= MaxValueBaseUnit)
            {
                baseUnitValue = value;
                SetValue(BaseUnitValueProperty, value);
                OnPropertyChanged(nameof(BaseUnitValue));
                SetValueText(value, false);

                if (ValueChangedCommand?.CanExecute(null) ?? false)
                    ValueChangedCommand.Execute(null);
            }
        }
    }

    public int SelectedUnitIndex
    {
        get => selectedUnitIndex;
        set
        {
            if (value >= 0 && value < UnitConverter.Units.Length)
            {
                selectedUnitIndex = value;
                SetValue(SelectedUnitIndexProperty, value);
                UnitPicker.SelectedIndex = value;
            }
        }
    }

    public double MinValueBaseUnit { get; set; } = 0;

    public double MaxValueBaseUnit { get; set; } = 1000;

    public bool AcceptIntOnly { get; set; } = false;

    public int DisplayPrecision { get; set; } = 3;

    public bool IsReadOnly
    {
        get => UnitEntry.IsReadOnly;
        set => UnitEntry.IsReadOnly = value;
    }

    public UnitEntryControl()
    {
        InitializeComponent();
        UnitEntry.Text = string.Empty;
        unitConverter = StaticConverters.LengthUnitConverter;
        UnitPicker.ItemsSource = unitConverter.Units;
        UnitPicker.SelectedItem = unitConverter.Units[SelectedUnitIndex];
        SetValueText(baseUnitValue, false);
    }

    private void UnitEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(UnitEntry.Text) || !ParseHelper.TryParseDoubleDifferentCulture(UnitEntry.Text, out double newValue) || newValue != selectedUnitValue)
        {
            SetValueText(baseUnitValue, false);
            return;
        }
    }

    private void UnitEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!isInputFromUser)
            return;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            return;

        ReadOnlySpan<char> newTextSpan = e.NewTextValue.AsSpan().Trim();
        if (MinValueBaseUnit < 0 && newTextSpan.Equals("-", StringComparison.Ordinal))
            return;

        if (!AcceptIntOnly && (newTextSpan.IsEmpty || newTextSpan[^1].Equals(ParseHelper.Dot) || newTextSpan[^1].Equals(ParseHelper.Comma)))
            return;

        if (!ParseHelper.TryParseDoubleDifferentCulture(e.NewTextValue, out _))
        {
            UnitEntry.Text = e.OldTextValue;
            return;
        }

        SetValueFromText(true);
    }

    private void PickerIndexChanged(object sender, EventArgs e)
    {
        int index = UnitPicker.SelectedIndex;

        if (index >= 0 && index < UnitConverter.Units.Length)
        {
            SelectedUnitIndex = index;
            SetValueText(baseUnitValue, isInputFromUser);
        }
    }

    private void SetValueFromText(bool isInputFromUser)
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(UnitEntry.Text, out double newValue))
        {
            if (SetBaseUnitValueAndNotify(UnitConverter.ConvertToBaseUnit(newValue, UnitConverter.Units[SelectedUnitIndex]), isInputFromUser))
                return;
        }

        SetValueText(baseUnitValue, isInputFromUser);
    }

    private bool SetBaseUnitValueAndNotify(double value, bool isInputFromUser)
    {
        if (value != baseUnitValue && value >= MinValueBaseUnit && value <= MaxValueBaseUnit)
        {
            baseUnitValue = value;
            SetValue(BaseUnitValueProperty, baseUnitValue);
            OnPropertyChanged(nameof(BaseUnitValue));
            SetValueText(value, isInputFromUser);

            if (ValueChangedCommand?.CanExecute(null) ?? false)
                ValueChangedCommand.Execute(null);

            return true;
        }

        return false;
    }

    private void SetValueText(double value, bool isInputFromUser)
    {
        this.isInputFromUser = isInputFromUser;

        value = UnitConverter.ConvertBaseToSelectedUnit(value, UnitConverter.Units[SelectedUnitIndex]);
        value = AcceptIntOnly ? Math.Round(value, 0) : Math.Round(value, DisplayPrecision);

        selectedUnitValue = value;
        UnitEntry.Text = value.ToString();

        this.isInputFromUser = true;
    }
}