using System.Windows.Input;

namespace Photography_Tools.Components.Controls.NumericEntry;

public sealed partial class IntEntry : NumEntryBase<int>, IDisposable
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

    protected override Entry NumEntry { get => MainIntEntry; }

    public override BindableProperty ValueChangedCommandProp => ValueChangedCommandProperty;

    public override BindableProperty EntryValueProp => EntryValueProperty;

    public IntEntry()
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

    protected override int CustomizeNewValue(int newValue)
    {
        return newValue;
    }
}
