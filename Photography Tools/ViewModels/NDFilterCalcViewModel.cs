using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Photography_Tools.ViewModels;

public partial class NDFilterCalcViewModel : ObservableObject
{
    private readonly IPhotographyCalculationsService photographyCalculationsService;
    private readonly INDFiltersDataAccess ndFiltersDataAccess;

    private TimeSpan inputTime, resultTime;

    [ObservableProperty]
    private string inputTimeText, resultTimeText, filterToAddName;

    [ObservableProperty]
    ObservableCollection<NDFilter> ndFilters;

    public ImmutableArray<string> AvaliableNDFiltersNames { get; }

    public ImmutableArray<string> AllShutterSpeedsNames { get; }

    public NDFilterCalcViewModel(IPhotographyCalculationsService photographyCalculationsService, INDFiltersDataAccess ndFiltersDataAccess)
    {
        this.photographyCalculationsService = photographyCalculationsService;
        this.ndFiltersDataAccess = ndFiltersDataAccess;

        ndFilters = [];

        AllShutterSpeedsNames = ShutterSpeedConst.AllShutterSpeedsNamesSorted;
        AvaliableNDFiltersNames = ndFiltersDataAccess.GetFilterNames();
        
        FilterToAddName = AvaliableNDFiltersNames[0];
        InputTimeText = ShutterSpeedConst.AllShutterSpeedsNamesSorted[9];
        ResultTimeText = string.Empty;

        CalculateTime();
    }

    [RelayCommand]
    private void CalculateTime() 
    {
        if (ShutterSpeedConst.AllShutterSpeeds.TryGetValue(InputTimeText, out double time) && time <= TimeSpan.MaxValue.TotalSeconds)
        {
            inputTime = TimeSpan.FromSeconds(time);
            try
            {
                resultTime = photographyCalculationsService.CalculateTimeWithNDFilters(inputTime, NdFilters);
                ResultTimeText = GetTimeText(resultTime);
            }
            catch (OverflowException)
            {
                ResultTimeText = "Invalid input data";
            }
        }
        else if(!string.IsNullOrEmpty(InputTimeText))
        {
            InputTimeText = inputTime.TotalSeconds.ToString();
            ResultTimeText = GetTimeText(resultTime);
        }
    }

    [RelayCommand]
    private void AddFilter(string filterName)
    {
        NdFilters.Add(ndFiltersDataAccess.GetFilter(filterName));
        CalculateTime();
    }

    [RelayCommand]
    private void RemoveFilter(NDFilter filter)
    {
        NdFilters.Remove(filter);
        CalculateTime();
    }

    [RelayCommand]
    private void ClearAllFilters()
    {
        NdFilters.Clear();
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