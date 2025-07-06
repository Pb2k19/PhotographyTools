using System.Windows.Input;

namespace Photography_Tools.Components.Controls.UnitEntry;

public sealed partial class UnitEntryControl : NumEntryBase<double>, IDisposable
{
    public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(
        nameof(ValueChangedCommand),
        typeof(ICommand),
        typeof(UnitEntryControl));

    public static readonly BindableProperty EntryValueProperty = BindableProperty.Create(
        nameof(EntryValue),
        typeof(double),
        typeof(UnitEntryControl),
        defaultValue: 0.0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (UnitEntryControl)bindable;
            control.EntryValue = (double)newValue;
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

    private IUnitConverter unitConverter;
    private int selectedUnitIndex;
    private double selectedUnitValue;

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

    public bool AcceptIntOnly { get; set; } = false;

    public int DisplayPrecision { get; set; } = 3;

    protected override Entry NumEntry => UnitEntry;

    public override BindableProperty ValueChangedCommandProp => ValueChangedCommandProperty;

    public override BindableProperty EntryValueProp => EntryValueProperty;

    public UnitEntryControl()
    {
        MinValue = 0;
        MaxValue = 1000;
        InitializeComponent();
        UnitEntry.Text = string.Empty;
        unitConverter = StaticConverters.LengthUnitConverter;
        UnitPicker.ItemsSource = unitConverter.Units;
        UnitPicker.SelectedItem = unitConverter.Units[SelectedUnitIndex];
        SetValueText(entryValue, false);
    }

    ~UnitEntryControl() => Dispose(false);

    private void UnitEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(UnitEntry.Text) || !ParseHelper.TryParseDifferentCulture(UnitEntry.Text, out double newValue) || newValue != selectedUnitValue)
        {
            SetValueText(entryValue, false);
        }
    }

    private async void UnitEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        await OnTextChanged(e.NewTextValue, e.OldTextValue);
    }

    private void PickerIndexChanged(object sender, EventArgs e)
    {
        int index = UnitPicker.SelectedIndex;

        if (index >= 0 && index < UnitConverter.Units.Length)
        {
            SelectedUnitIndex = index;
            SetValueText(entryValue, false);
        }
    }

    protected override void SetValueText(double value, bool isInputFromUser)
    {
        this.isInputFromUser = isInputFromUser;

        value = Math.Round(UnitConverter.ConvertBaseToSelectedUnit(value, UnitConverter.Units[SelectedUnitIndex]), AcceptIntOnly ? 0 : DisplayPrecision);
        selectedUnitValue = value;
        UnitEntry.Text = value.ToString();

        this.isInputFromUser = true;
    }

    protected override double CustomizeNewValue(double newValue)
    {
        return UnitConverter.ConvertToBaseUnit(newValue, UnitConverter.Units[SelectedUnitIndex]);
    }
}