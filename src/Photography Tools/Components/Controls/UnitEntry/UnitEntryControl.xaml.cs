using System.Windows.Input;

namespace Photography_Tools.Components.Controls.UnitEntry;

public partial class UnitEntryControl : ContentView
{
    private IUnitConverter unitConverter;
    private int selectedUnitIndex;
    private double baseUnitValue;

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
                SetValueText(value);

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
        SetValueText(baseUnitValue);
    }

    private void UnitEntry_Completed(object sender, EventArgs e)
    {
        SetValueFromText();
    }

    private void UnitEntry_Unfocused(object sender, FocusEventArgs e)
    {
        SetValueFromText();
    }

    private void UnitEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            return;

        if (MinValueBaseUnit < 0 && e.NewTextValue.AsSpan().Trim().Equals("-", StringComparison.Ordinal))
            return;

        if (!ParseHelper.TryParseDoubleDifferentCulture(e.NewTextValue, out _))
        {
            UnitEntry.Text = e.OldTextValue;
            return;
        }
    }

    private void PickerIndexChanged(object sender, EventArgs e)
    {
        int index = UnitPicker.SelectedIndex;

        if (index >= 0 && index < UnitConverter.Units.Length)
        {
            SelectedUnitIndex = index;
            SetValueText(baseUnitValue);
        }
    }

    private void SetValueFromText()
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(UnitEntry.Text, out double newValue))
        {
            if (SetBaseUnitValueAndNotify(UnitConverter.ConvertToBaseUnit(newValue, UnitConverter.Units[SelectedUnitIndex])))
                return;
        }

        SetValueText(baseUnitValue);
    }

    private bool SetBaseUnitValueAndNotify(double value)
    {
        if (value != baseUnitValue && value >= MinValueBaseUnit && value <= MaxValueBaseUnit)
        {
            baseUnitValue = value;
            SetValue(BaseUnitValueProperty, baseUnitValue);
            OnPropertyChanged(nameof(BaseUnitValue));
            SetValueText(value);

            if (ValueChangedCommand?.CanExecute(null) ?? false)
                ValueChangedCommand.Execute(null);

            return true;
        }

        return false;
    }

    private void SetValueText(double value)
    {
        value = UnitConverter.ConvertBaseToSelectedUnit(value, UnitConverter.Units[SelectedUnitIndex]);
        value = AcceptIntOnly ? Math.Round(value, 0) : Math.Round(value, DisplayPrecision);

        UnitEntry.Text = value.ToString();
    }
}