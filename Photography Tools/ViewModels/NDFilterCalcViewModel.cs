using System.Collections.Immutable;

namespace Photography_Tools.ViewModels;

public partial class NDFilterCalcViewModel : ObservableObject
{
    private readonly IPhotographyCalculationsService photographyCalculationsService;
    private readonly IPreferencesService preferencesServcie;
    private readonly INDFiltersDataAccess ndFiltersDataAccess;

    private TimeSpan inputTime, resultTime;

    [ObservableProperty]
    private string resultTimeText = string.Empty, filterToAddName;

    [ObservableProperty]
    private NDFilterCalcUserInput userInput;

    public ImmutableArray<string> AvaliableNDFiltersNames { get; }

    public ImmutableArray<string> AllShutterSpeedsNames { get; }

    public NDFilterCalcViewModel(IPhotographyCalculationsService photographyCalculationsService, IPreferencesService preferencesServcie, INDFiltersDataAccess ndFiltersDataAccess)
    {
        this.photographyCalculationsService = photographyCalculationsService;
        this.preferencesServcie = preferencesServcie;
        this.ndFiltersDataAccess = ndFiltersDataAccess;

        AllShutterSpeedsNames = ShutterSpeedConst.AllShutterSpeedsNamesSorted;
        AvaliableNDFiltersNames = ndFiltersDataAccess.GetFilterNames();
        FilterToAddName = AvaliableNDFiltersNames[0];

        NDFilterCalcUserInput? input = preferencesServcie.GetDeserailizedPreference<NDFilterCalcUserInput>(PreferencesKeys.NDFilterCalcUserInputPreferencesKey);
        userInput = input is not null ? input : new() { TimeText = ShutterSpeedConst.AllShutterSpeedsNamesSorted[9], NdFilters = [] };

        CalculateTime();
    }

    [RelayCommand]
    private void OnDisappearing()
    {
        preferencesServcie.SerializedAndSetPreference(PreferencesKeys.NDFilterCalcUserInputPreferencesKey, UserInput);
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

        if (time.Days > 0)
            return $"{time.Days} days, {time.Hours}h, {time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else if (time.Hours > 0)
            return $"{time.Hours}h, {time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else if (time.Minutes > 0)
            return $"{time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else
            return $"{time.Seconds}s, {time.Milliseconds}ms";
    }
}