using System.Numerics;
using System.Windows.Input;

namespace Photography_Tools.Components.Controls;

public abstract partial class NumEntryBase<T> : ContentView, IDisposable where T : notnull, INumber<T>
{
    protected CancellationTokenSource? cts = null;
    protected T entryValue;
    protected bool isInputFromUser = true, isTokenSourceDisposed = false;

    protected abstract Entry NumEntry { get; }

    public abstract BindableProperty ValueChangedCommandProp { get; }

    public abstract BindableProperty EntryValueProp { get; }

    public ICommand ValueChangedCommand
    {
        get => (ICommand)GetValue(ValueChangedCommandProp);
        set => SetValue(ValueChangedCommandProp, value);
    }

    public T MinValue { get; set; }

    public T MaxValue { get; set; }

    public int Precision { get; set; } = 3;

    public T EntryValue
    {
        get => entryValue;
        set
        {
            if (entryValue == value)
                return;

            entryValue = value;
            SetValue(EntryValueProp, value);
            OnPropertyChanged(nameof(EntryValue));
            SetValueText(value, false);

            if (ValueChangedCommand?.CanExecute(null) ?? false)
                ValueChangedCommand.Execute(null);
        }
    }

    public bool IsReadOnly
    {
        get => NumEntry.IsReadOnly;
        set => NumEntry.IsReadOnly = value;
    }

    public NumEntryBase()
    {
        entryValue = T.Zero;
        MinValue = T.Zero;
        MaxValue = T.One;
    }

    protected virtual void OnUnfocused()
    {
        if (string.IsNullOrEmpty(NumEntry.Text) || !ParseHelper.TryParseDifferentCulture(NumEntry.Text, out T? newValue) || newValue != EntryValue)
        {
            SetValueText(EntryValue, false);
            return;
        }
    }

    protected async Task OnTextChanged(string newTextValue, string oldTextValue)
    {
        if (!isInputFromUser || IsReadOnly)
            return;

        if (cts is not null)
        {
            await cts.CancelAsync();
            cts.Dispose();
        }

        cts = new();
        if (string.IsNullOrWhiteSpace(newTextValue))
            return;

        Task t = Task.Delay(350, cts.Token);
        await t.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing | ConfigureAwaitOptions.ContinueOnCapturedContext);

        if (t.Status is not TaskStatus.RanToCompletion)
            return;

        ReadOnlySpan<char> newTextSpan = newTextValue.AsSpan().Trim();

        if (newTextSpan.IsEmpty)
            return;

        if (MinValue < T.Zero && newTextSpan.Length == 1 && ParseHelper.NegativeSignSearchValues.Contains(newTextSpan[0]))
            return;

        if (ParseHelper.DecimalSeparatorSearchValues.Contains(newTextSpan[^1]))
            return;

        if (!ParseHelper.TryParseDifferentCulture<double>(newTextSpan, out _))
        {
            NumEntry.Text = oldTextValue;
            return;
        }

        SetValueFromText(true);
    }

    protected void SetValueFromText(bool isInputFromUser)
    {
        if (ParseHelper.TryParseDifferentCulture(NumEntry.Text, out T? newValue))
        {
            if (SetValueAndNotify(CustomizeNewValue(newValue), isInputFromUser))
                return;
        }

        SetValueText(entryValue, isInputFromUser);
    }

    protected bool SetValueAndNotify(T value, bool isInputFromUser)
    {
        if (value == EntryValue)
            return false;

        if (isInputFromUser)
        {
            if (value < MinValue)
                value = MinValue;
            else if (value > MaxValue)
                value = MaxValue;
        }

        entryValue = value;
        SetValue(EntryValueProp, entryValue);
        OnPropertyChanged(nameof(EntryValue));
        SetValueText(value, isInputFromUser);

        if (ValueChangedCommand?.CanExecute(null) ?? false)
            ValueChangedCommand.Execute(null);

        return true;
    }

    protected abstract T CustomizeNewValue(T? newValue);

    protected virtual void SetValueText(T value, bool isInputFromUser)
    {
        this.isInputFromUser = isInputFromUser;
        NumEntry.Text = value?.ToString();
        this.isInputFromUser = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (isTokenSourceDisposed)
            return;

        if (disposing)
        {
            cts?.Dispose();
        }

        isTokenSourceDisposed = true;
    }
}
