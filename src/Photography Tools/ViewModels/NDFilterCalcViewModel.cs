using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class NDFilterCalcViewModel : SaveableViewModel
{
    private readonly IPhotographyCalculationsService photographyCalculationsService;
    private readonly INDFiltersDataAccess ndFiltersDataAccess;

    private bool isCountdownRunning = false;
    private CancellationTokenSource? cancellationTokenSource;

    private TimeSpan inputTime, resultTime;

    [ObservableProperty]
    private string resultTimeText = string.Empty, filterToAddName;

    [ObservableProperty]
    private NDFilterCalcUserInput userInput;

    public ImmutableArray<string> AvaliableNDFiltersNames { get; }

    public ImmutableArray<string> AllShutterSpeedsNames { get; }

    public NDFilterCalcViewModel(IPhotographyCalculationsService photographyCalculationsService, IPreferencesService preferencesService, INDFiltersDataAccess ndFiltersDataAccess) : base(preferencesService)
    {
        this.photographyCalculationsService = photographyCalculationsService;
        this.ndFiltersDataAccess = ndFiltersDataAccess;

        AllShutterSpeedsNames = ShutterSpeedConst.AllShutterSpeedsNamesSorted;
        AvaliableNDFiltersNames = ndFiltersDataAccess.GetFilterNames();
        FilterToAddName = AvaliableNDFiltersNames[0];

        NDFilterCalcUserInput? input = preferencesService.GetDeserailizedPreference<NDFilterCalcUserInput>(PreferencesKeys.NDFilterCalcUserInputPreferencesKey);
        userInput = input is not null && input.Validate() ? input : new() { TimeText = ShutterSpeedConst.AllShutterSpeedsNamesSorted[9], NdFilters = [] };

        CalculateTime();
    }

    [RelayCommand]
    private void CalculateTime()
    {
        if (ShutterSpeedConst.AllShutterSpeeds.TryGetValue(UserInput.TimeText, out double time) && time <= TimeSpan.MaxValue.TotalSeconds)
        {
            inputTime = TimeSpan.FromSeconds(time);
            try
            {
                resultTime = photographyCalculationsService.CalculateTimeWithNDFilters(inputTime, UserInput.NdFilters);
                ResultTimeText = GetTimeText(resultTime);
            }
            catch (OverflowException)
            {
                ResultTimeText = "Invalid input data";
            }
        }
        else if (!string.IsNullOrEmpty(UserInput.TimeText))
        {
            UserInput.TimeText = inputTime.TotalSeconds.ToString();
            ResultTimeText = GetTimeText(resultTime);
        }
    }

    [RelayCommand]
    private void AddFilter(string filterName)
    {
        UserInput.NdFilters.Add(ndFiltersDataAccess.GetFilter(filterName));
        CalculateTime();
    }

    [RelayCommand]
    private void RemoveFilter(NDFilter filter)
    {
        UserInput.NdFilters.Remove(filter);
        CalculateTime();
    }

    [RelayCommand]
    private void ClearAllFilters()
    {
        UserInput.NdFilters.Clear();
        CalculateTime();
    }

    [RelayCommand]
    private async Task StartCountdownAsync()
    {
        if (isCountdownRunning)
            return;

        isCountdownRunning = true;
        try
        {
            cancellationTokenSource ??= new();
            await CountdownAsync(cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            ResultTimeText = "Canceled";
            await Task.Delay(2000);
        }
        catch (ArgumentOutOfRangeException)
        {
            ResultTimeText = "Exposure With Filters is too big";
            await Task.Delay(2000);
        }
        finally
        {
            ResultTimeText = GetTimeText(resultTime);
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            isCountdownRunning = false;
        }
    }

    [RelayCommand]
    private void StopCountdown()
    {
        if (isCountdownRunning && cancellationTokenSource is not null && !cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
    }

    private async Task CountdownAsync(CancellationToken cancellationToken)
    {
        DateTime endDateTime = DateTime.Now.Add(resultTime);

        while (endDateTime > DateTime.Now)
        {
            ResultTimeText = GetTimeTextCore(endDateTime - DateTime.Now);
            await Task.Delay(32, cancellationToken);
        }

        ResultTimeText = "STOP";
        await Task.Delay(3000, cancellationToken);
    }

    private static string GetTimeText(TimeSpan time)
    {
        if (time == TimeSpan.Zero)
            return "0s";

        if (time.TotalSeconds < 10)
        {
#if DEBUG
            return $"{ShutterSpeedConst.AllShutterSpeeds.Aggregate((s1, s2) => Math.Abs(s1.Value - time.TotalSeconds) < Math.Abs(s2.Value - time.TotalSeconds) ? s1 : s2).Key} ({time.TotalSeconds}s)";
#else
            return ShutterSpeedConst.AllShutterSpeeds.Aggregate((s1, s2) => Math.Abs(s1.Value - time.TotalSeconds) < Math.Abs(s2.Value - time.TotalSeconds) ? s1 : s2).Key;
#endif
        }

        return GetTimeTextCore(time);
    }

    public static string GetTimeTextCore(TimeSpan time)
    {
        if (time.Days > 0)
            return $"{time.Days} days, {time.Hours}h, {time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else if (time.Hours > 0)
            return $"{time.Hours}h, {time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else if (time.Minutes > 0)
            return $"{time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else
            return $"{time.Seconds}s, {time.Milliseconds}ms";
    }

    protected override void SaveUserInput()
    {
        if (UserInput.Validate())
            preferencesService.SerializedAndSetPreference(PreferencesKeys.NDFilterCalcUserInputPreferencesKey, UserInput);
    }
}