using System.Windows.Input;

namespace Photography_Tools.Components.Controls.NumericEntry;

public partial class DoubleEntry : ContentView
{
    public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(
        nameof(ValueChangedCommand),
        typeof(ICommand),
        typeof(DoubleEntry));

    public static readonly BindableProperty EntryValueProperty = BindableProperty.Create(
        nameof(EntryValue),
        typeof(double),
        typeof(DoubleEntry),
        defaultValue: 0.0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (DoubleEntry)bindable;
            control.EntryValue = (double)newValue;
        });

    private double entryValue;

    public ICommand? ValueChangedCommand
    {
        get => (ICommand)GetValue(ValueChangedCommandProperty);
        set => SetValue(ValueChangedCommandProperty, value);
    }

    public double MinValue { get; set; } = 0;

    public double MaxValue { get; set; } = 10_000;

    public int Precision { get; set; } = 3;

    public double EntryValue
    {
        get => entryValue;
        set
        {
            if (entryValue == value || value < MinValue || value > MaxValue)
                return;

            entryValue = value;
            SetValue(EntryValueProperty, value);
            OnPropertyChanged(nameof(EntryValue));
            SetValueText(value);

            if (ValueChangedCommand?.CanExecute(null) ?? false)
                ValueChangedCommand.Execute(null);
        }
    }

    public DoubleEntry()
    {
        InitializeComponent();
    }

    private void NumEntry_Completed(object sender, EventArgs e)
    {
        SetValueFromText();
    }

    private void NumEntry_Unfocused(object sender, FocusEventArgs e)
    {
        SetValueFromText();
    }

    private void NumEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            return;

        if (MinValue < 0 && e.NewTextValue.AsSpan().Trim().Equals("-", StringComparison.Ordinal))
            return;

        if (!ParseHelper.TryParseDoubleDifferentCulture(e.NewTextValue, out _))
        {
            NumEntry.Text = e.OldTextValue;
            return;
        }
    }

    private void SetValueFromText()
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(NumEntry.Text, out double newValue))
        {
            if (SetValueAndNotify(Math.Round(newValue, Precision)))
                return;
        }

        SetValueText(entryValue);
    }

    private bool SetValueAndNotify(double value)
    {
        if (value == entryValue || value < MinValue || value > MaxValue)
            return false;

        entryValue = value;
        SetValue(EntryValueProperty, entryValue);
        OnPropertyChanged(nameof(EntryValue));
        SetValueText(value);

        if (ValueChangedCommand?.CanExecute(null) ?? false)
            ValueChangedCommand.Execute(null);

        return true;
    }

    private void SetValueText(double value)
    {
        NumEntry.Text = value.ToString();
    }
}