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

    private bool isInputFromUser = true;

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
            SetValueText(value, false);

            if (ValueChangedCommand?.CanExecute(null) ?? false)
                ValueChangedCommand.Execute(null);
        }
    }

    public IntEntry()
    {
        InitializeComponent();
    }

    private void NumEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(NumEntry.Text) || !int.TryParse(NumEntry.Text, out int newValue) || newValue != EntryValue)
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

        if (MinValue < 0 && e.NewTextValue.AsSpan().Trim().Equals("-", StringComparison.Ordinal))
            return;

        if (!int.TryParse(e.NewTextValue, out _))
        {
            NumEntry.Text = e.OldTextValue;
            return;
        }

        SetValueFromText(true);
    }

    private void SetValueFromText(bool isInputFromUser)
    {
        if (int.TryParse(NumEntry.Text, out int newValue))
        {
            if (SetValueAndNotify(newValue, isInputFromUser))
                return;
        }

        SetValueText(entryValue, isInputFromUser);
    }

    private bool SetValueAndNotify(int value, bool isInputFromUser)
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

    private void SetValueText(int value, bool isInputFromUser)
    {
        this.isInputFromUser = isInputFromUser;
        NumEntry.Text = value.ToString();
        this.isInputFromUser = true;
    }
}