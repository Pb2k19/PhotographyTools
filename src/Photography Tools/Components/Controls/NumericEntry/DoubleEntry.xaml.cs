using System.Windows.Input;

namespace Photography_Tools.Components.Controls.NumericEntry;

public sealed partial class DoubleEntry : NumEntryBase<double>, IDisposable
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

    protected override Entry NumEntry { get => MainDoubleEntry; }

    public override BindableProperty ValueChangedCommandProp => ValueChangedCommandProperty;

    public override BindableProperty EntryValueProp => EntryValueProperty;

    public DoubleEntry()
    {
        MinValue = 0;
        MaxValue = 10_000;

        InitializeComponent();
    }

    private void NumEntry_Unfocused(object sender, FocusEventArgs e)
    {
        OnUnfocused();
    }

    private async void NumEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        await OnTextChanged(e.NewTextValue, e.OldTextValue);
    }

    protected override double CustomizeNewValue(double newValue)
    {
        return Math.Round(newValue, Precision);
    }
}