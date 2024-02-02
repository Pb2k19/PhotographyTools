using Photography_Tools.Const;
using Photography_Tools.Helpers;
using System.Collections.Immutable;
using System.Windows.Input;

namespace Photography_Tools.Components.Controls;

public partial class LengthEntryControl : ContentView
{
    private double lengthMM;
    private string previousLengthText = string.Empty;

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

    public ICommand LengthValueChangedCommand
    {
        get => (ICommand)GetValue(LengthValueChangedCommandProperty);
        set => SetValue(LengthValueChangedCommandProperty, value);
    }
    
    public ICommand UnitChangedCommand
    {
        get => (ICommand)GetValue(UnitChangedCommandProperty);
        set => SetValue(UnitChangedCommandProperty, value);
    }

    public double LengthMM
    {
        get => lengthMM;
        set
        {
            value = AcceptIntOnly ? Math.Round(value, 0) : Math.Round(value, DisplayPrecison);

            if (lengthMM != value && value >= MinLengthMM && value <= MaxLengthMM)
            {
                lengthMM = value;
                SetValue(LengthMMProperty, value);
                OnPropertyChanged(nameof(LengthMM));
                SetLenghtText(value);

                if (LengthValueChangedCommand.CanExecute(null))
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

    public int DisplayPrecison { get; set; } = 3;

    private ImmutableArray<string> LengthUnits { get; }

    public LengthEntryControl()
    {
        InitializeComponent();
        LengthEntry.Text = string.Empty;
        LengthUnits = UnitConst.LengthUnits;
        UnitPicker.ItemsSource = LengthUnits;
        SetLenghtText(lengthMM);
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
            SetLengthFromText();

            if (UnitChangedCommand?.CanExecute(null) ?? false)
                UnitChangedCommand.Execute(this);
        }
    }

    private void SetLengthFromText()
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(LengthEntry.Text, out double newValue))
        {
            newValue = AcceptIntOnly ? Math.Round(newValue, 0) : newValue;

            if (newValue >= MinLengthMM && newValue <= MaxLengthMM)
            {
                double newValueMM = UnitHelper.ConvertUnitsToMM(newValue, SelectedUnit);
                if (newValueMM == lengthMM)
                    return;

                lengthMM = newValueMM;
                SetValue(LengthMMProperty, lengthMM);
                OnPropertyChanged(nameof(LengthMM));
                SetLenghtText(Math.Round(newValue, DisplayPrecison).ToString());

                if (LengthValueChangedCommand?.CanExecute(null) ?? false)
                    LengthValueChangedCommand.Execute(null);

                return;
            }
        }

        SetLenghtText(previousLengthText);
    }

    private void SetLenghtText(string value)
    {
        previousLengthText = value;
        LengthEntry.Text = value;
    }

    private void SetLenghtText(double value)
    {
        SetLenghtText(Math.Round(UnitHelper.ConvertMMToUnits(value, SelectedUnit), DisplayPrecison).ToString());
    }
}