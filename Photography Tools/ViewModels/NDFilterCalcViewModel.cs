using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Photography_Tools.ViewModels;

public partial class NDFilterCalcViewModel : ObservableObject
{
    private readonly IPhotographyCalculationsService photographyCalculationsService;
    private readonly INDFiltersDataAccess ndFiltersDataAccess;

    private TimeSpan inputTime, resultTime;

    [ObservableProperty]
    private string inputTimeText, resultTimeText;

    [ObservableProperty]
    ObservableCollection<NDFilter> ndFilters;

    public ImmutableArray<string> AvaliableNDFiltersNames { get; }

    public NDFilterCalcViewModel(IPhotographyCalculationsService photographyCalculationsService, INDFiltersDataAccess ndFiltersDataAccess)
    {
        this.photographyCalculationsService = photographyCalculationsService;
        this.ndFiltersDataAccess = ndFiltersDataAccess;
        AvaliableNDFiltersNames = ndFiltersDataAccess.GetFilterNames();

        ndFilters = new(ndFiltersDataAccess.GetFilters().Take(5));

        inputTime = TimeSpan.FromSeconds(1);
        InputTimeText = "1";
        ResultTimeText = "1";

        CalculateTime();
    }

    [RelayCommand]
    private void CalculateTime() 
    {
        if (ParseHelper.TryParseDoubleDifferentCulture(InputTimeText, out double time) && time <= TimeSpan.MaxValue.TotalSeconds)
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
    private void AddFilter(string filterName) => NdFilters.Add(ndFiltersDataAccess.GetFilter(filterName));

    [RelayCommand]
    private void RemoveFilter(string filterName) => NdFilters.Remove(ndFiltersDataAccess.GetFilter(filterName));

    [RelayCommand]
    private void ClearAllFilters() => NdFilters.Clear();

    private static string GetTimeText(TimeSpan time)
    {
        if (time.Days > 0)
            return $"{time.Days} days, {time.Hours}h, {time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else if (time.Hours > 0)
            return $"{time.Hours}h, {time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else if (time.Minutes > 0)
            return $"{time.Minutes}m, {time.Seconds}s, {time.Milliseconds}ms";
        else if (time.Seconds > 0)
            return $"{time.Seconds}s, {time.Milliseconds}ms";
        else
            return $"{time.Milliseconds}ms";
    }
}