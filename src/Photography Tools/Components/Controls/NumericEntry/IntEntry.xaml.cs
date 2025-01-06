using System.Windows.Input;

namespace Photography_Tools.Components.Controls.NumericEntry;

public partial class IntEntry : ContentView
{
    public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(
        nameof(ValueChangedCommand),
        typeof(ICommand),
        typeof(IntEntry));

    public static readonly BindableProperty EntryValueProperty = BindableProperty.Create(
        nameof(EntryValue),
        typeof(int),
        typeof(IntEntry),
        defaultValue: 0,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (IntEntry)bindable;
            control.EntryValue = (int)newValue;
        });

    private int entryValue;

    public ICommand? ValueChangedCommand
    {
        get => (ICommand)GetValue(ValueChangedCommandProperty);
        set => SetValue(ValueChangedCommandProperty, value);
    }

    public int MinValue { get; set; } = 0;

    public int MaxValue { get; set; } = 10_000;

    public int EntryValue
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

    public IntEntry()
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

        if (!int.TryParse(e.NewTextValue, out _))
        {
            NumEntry.Text = e.OldTextValue;
            return;
        }
    }

    private void SetValueFromText()
    {
        if (int.TryParse(NumEntry.Text, out int newValue))
        {
            if (SetValueAndNotify(newValue))
                return;
        }

        SetValueText(entryValue);
    }

    private bool SetValueAndNotify(int value)
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

    private void SetValueText(int value)
    {
        NumEntry.Text = value.ToString();
    }
}