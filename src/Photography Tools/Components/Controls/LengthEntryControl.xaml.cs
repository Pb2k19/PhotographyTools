using System.Collections.Immutable;
using System.Windows.Input;
#if DEBUG
using System.Diagnostics;
#endif

namespace Photography_Tools.Components.Controls;

public partial class LengthEntryControl : ContentView
{
    private int selectedUnitIndex;
    private double lengthMM;

    #region BindableProperties
    public static readonly BindableProperty LengthValueChangedCommandProperty = BindableProperty.Create(
        nameof(LengthValueChangedCommand),
        typeof(ICommand),
        typeof(LengthEntryControl));

    public static readonly BindableProperty LengthMMProperty = BindableProperty.Create(
        nameof(LengthMM),
        typeof(double),
        typeof(LengthEntryControl),
        defaultValue: 0.0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (LengthEntryControl)bindable;
            control.LengthMM = (double)newValue;
        });

    public static readonly BindableProperty SelectedUnitIndexProperty = BindableProperty.Create(
        nameof(SelectedUnitIndex),
        typeof(int),
        typeof(LengthEntryControl),
        defaultValue: 0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (LengthEntryControl)bindable;
            control.SelectedUnitIndex = (int)newValue;
        });
    #endregion

    public ICommand? LengthValueChangedCommand
    {
        get => (ICommand)GetValue(LengthValueChangedCommandProperty);
        set => SetValue(LengthValueChangedCommandProperty, value);
    }

    public double LengthMM
    {
        get => lengthMM;
        set
        {
            if (lengthMM != value && value >= MinLengthMM && value <= MaxLengthMM)
            {
                lengthMM = value;
                SetValue(LengthMMProperty, value);
                OnPropertyChanged(nameof(LengthMM));
                SetLengthText(value);

                if (LengthValueChangedCommand?.CanExecute(null) ?? false)
                    LengthValueChangedCommand.Execute(null);
            }
        }
    }

    public int SelectedUnitIndex
    {
        get => selectedUnitIndex;
        set
        {
            if (value >= 0 && value < LengthUnits.Length)
            {
                selectedUnitIndex = value;
                SetValue(SelectedUnitIndexProperty, value);
                UnitPicker.SelectedIndex = value;
            }
        }
    }

    public double MinLengthMM { get; set; } = 0;

    public double MaxLengthMM { get; set; } = 1000;

    public bool AcceptIntOnly { get; set; } = false;

    public int DisplayPrecision { get; set; } = 3;

    public bool IsReadOnly
    {
        get => LengthEntry.IsReadOnly;
        set => LengthEntry.IsReadOnly = value;
    }

    private ImmutableArray<string> LengthUnits { get; }

    public LengthEntryControl()
    {
        InitializeComponent();
        LengthEntry.Text = string.Empty;
        LengthUnits = UnitConst.LengthUnits;
        UnitPicker.ItemsSource = LengthUnits;
        SetLengthText(lengthMM);
        UnitPicker.SelectedItem = LengthUnits[SelectedUnitIndex];
    }

    private void LengthEntry_Completed(object sender, EventArgs e)
    {
        SetLengthFromText();
    }

    private void LengthEntry_Unfocused(object sender, FocusEventArgs e)
    {
        SetLengthFromText();
    }

    private void PickerIndexChanged(object sender, EventArgs e)
    {
        int index = UnitPicker.SelectedIndex;

        if (index >= 0 && index < LengthUnits.Length)
        {
            SelectedUnitIndex = index;
            SetLengthText(lengthMM);
        }
    }

    private void SetLengthFromText()
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(LengthEntry.Text, out double newValue))
        {
            if (SetLenghtMMAndNotify(UnitHelper.ConvertUnitsToMM(newValue, LengthUnits[SelectedUnitIndex])))
                return;
        }

        SetLengthText(lengthMM);
    }

    private bool SetLenghtMMAndNotify(double value)
    {
        if (value != lengthMM && value >= MinLengthMM && value <= MaxLengthMM)
        {
            lengthMM = value;
            SetValue(LengthMMProperty, lengthMM);
            OnPropertyChanged(nameof(LengthMM));
            SetLengthText(value);

            if (LengthValueChangedCommand?.CanExecute(null) ?? false)
                LengthValueChangedCommand.Execute(null);

            return true;
        }

        return false;
    }

    private void SetLengthText(double value)
    {
        value = UnitHelper.ConvertMMToUnits(value, LengthUnits[SelectedUnitIndex]);
        value = AcceptIntOnly ? Math.Round(value, 0) : Math.Round(value, DisplayPrecision);

        LengthEntry.Text = value.ToString();
#if DEBUG
        Debug.WriteLine(lengthMM);
#endif
    }
}