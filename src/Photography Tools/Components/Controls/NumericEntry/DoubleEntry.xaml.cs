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

    private bool isInputFromUser = true;

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
            SetValueText(value, false);

            if (ValueChangedCommand?.CanExecute(null) ?? false)
                ValueChangedCommand.Execute(null);
        }
    }

    public DoubleEntry()
    {
        InitializeComponent();
    }

    private void NumEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(NumEntry.Text) || !ParseHelper.TryParseDoubleDifferentCulture(NumEntry.Text, out double newValue) || newValue != EntryValue)
        {
            SetValueText(EntryValue, false);
            return;
        }
    }

    private void NumEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!isInputFromUser)
            return;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            return;

        ReadOnlySpan<char> newTextSpan = e.NewTextValue.AsSpan().Trim();
        if (MinValue < 0 && newTextSpan.Equals("-", StringComparison.Ordinal))
            return;

        if (newTextSpan.IsEmpty || newTextSpan[^1].Equals(ParseHelper.Dot) || newTextSpan[^1].Equals(ParseHelper.Comma))
            return;

        if (!ParseHelper.TryParseDoubleDifferentCulture(newTextSpan, out _))
        {
            NumEntry.Text = e.OldTextValue;
            return;
        }

        SetValueFromText(true);
    }

    private void SetValueFromText(bool isInputFromUser)
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(NumEntry.Text, out double newValue))
        {
            if (SetValueAndNotify(Math.Round(newValue, Precision), isInputFromUser))
                return;
        }

        SetValueText(entryValue, isInputFromUser);
    }

    private bool SetValueAndNotify(double value, bool isInputFromUser)
    {
        if (value == entryValue || value < MinValue || value > MaxValue)
            return false;

        entryValue = value;
        SetValue(EntryValueProperty, entryValue);
        OnPropertyChanged(nameof(EntryValue));
        SetValueText(value, isInputFromUser);

        if (ValueChangedCommand?.CanExecute(null) ?? false)
            ValueChangedCommand.Execute(null);

        return true;
    }

    private void SetValueText(double value, bool isInputFromUser)
    {
        this.isInputFromUser = isInputFromUser;
        NumEntry.Text = value.ToString();
        this.isInputFromUser = true;
    }
}