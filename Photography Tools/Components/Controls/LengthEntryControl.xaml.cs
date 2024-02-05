using Photography_Tools.Const;
using Photography_Tools.Helpers;
using System.Collections.Immutable;
using System.Windows.Input;
#if DEBUG
using System.Diagnostics;
#endif

namespace Photography_Tools.Components.Controls;

public partial class LengthEntryControl : ContentView
{
    private double lengthMM;

    #region BindableProperties
    public static readonly BindableProperty LengthValueChangedCommandProperty = BindableProperty.Create(
        nameof(LengthValueChangedCommand),
        typeof(ICommand), 
        typeof(LengthEntryControl));

    public static readonly BindableProperty UnitChangedCommandProperty = BindableProperty.Create(
        nameof(UnitChangedCommand),
        typeof(ICommand),
        typeof(LengthEntryControl));

    public static readonly BindableProperty LengthMMProperty = BindableProperty.Create(
        nameof(LengthMM),
        typeof(double),
        typeof(LengthEntryControl),
        defaultValue: 0.0,
        defaultBindingMode:BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (LengthEntryControl)bindable;
            control.LengthMM = (double)newValue;
        });

    public static readonly BindableProperty SelectedUnitProperty = BindableProperty.Create(
        nameof(SelectedUnit),
        typeof(string),
        typeof(LengthEntryControl),
        defaultValue: UnitConst.LengthMM,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (LengthEntryControl)bindable;
            control.SelectedUnit = (string)newValue;
        });
    #endregion

    public ICommand? LengthValueChangedCommand
    {
        get => (ICommand)GetValue(LengthValueChangedCommandProperty);
        set => SetValue(LengthValueChangedCommandProperty, value);
    }
    
    public ICommand? UnitChangedCommand
    {
        get => (ICommand)GetValue(UnitChangedCommandProperty);
        set => SetValue(UnitChangedCommandProperty, value);
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

    public string SelectedUnit 
    {
        get => (string)GetValue(SelectedUnitProperty);
        set => SetValue(SelectedUnitProperty, value);
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
        if (LengthUnits.Length > 0)
            UnitPicker.SelectedItem = LengthUnits[0];
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
            SelectedUnit = LengthUnits[index];
            SetLengthText(lengthMM);

            if (UnitChangedCommand?.CanExecute(null) ?? false)
                UnitChangedCommand.Execute(this);
        }
    }

    private void SetLengthFromText()
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(LengthEntry.Text, out double newValue))
        {
            if (SetLenghtMMAndNotify(UnitHelper.ConvertUnitsToMM(newValue, SelectedUnit)))
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
        value = UnitHelper.ConvertMMToUnits(value, SelectedUnit);
        value = AcceptIntOnly ? Math.Round(value, 0) : Math.Round(value, DisplayPrecision);
           
        LengthEntry.Text = value.ToString();
#if DEBUG
        Debug.WriteLine(lengthMM);
#endif
    }
}