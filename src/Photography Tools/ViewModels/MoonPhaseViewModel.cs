using System.Collections.Frozen;
using System.Diagnostics;
using System.Text.Json;

namespace Photography_Tools.ViewModels;

public partial class MoonPhaseViewModel : ObservableObject
{
    private static readonly FrozenDictionary<string, string> MoonImages;

    private readonly IAstroDataService onlineAstroDataService;
    private readonly IAstroDataService offlineAstroDataService;
    private TimeSpan lastSelectedTime = TimeSpan.Zero;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan selectedTime = new(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, 0);

    [ObservableProperty]
    private string
        moonPhaseName = string.Empty,
        moonImage = string.Empty,
        illuminationPerc = string.Empty,
        moonAge = string.Empty,
        moonriseDate = string.Empty,
        moonsetDate = string.Empty;

    [ObservableProperty]
    private bool isNorthernHemisphere = true;


    static MoonPhaseViewModel()
    {
        MoonImages = new Dictionary<string, string>
        {
            {"New Moon", "🌑"},
            {"Waxing Crescent", "🌒"},
            {"First Quarter", "🌓"},
            {"Waxing Gibbous", "🌔"},
            {"Full Moon", "🌕"},
            {"Waning Gibbous", "🌖"},
            {"Third Quarter", "🌗"},
            {"Waning Crescent", "🌘"},
        }.ToFrozenDictionary();
    }

    public MoonPhaseViewModel([FromKeyedServices("onlineAstroData")] IAstroDataService onlineAstroDataService, [FromKeyedServices("offlineAstroData")] IAstroDataService offlineAstroDataService)
    {
        this.onlineAstroDataService = onlineAstroDataService;
        this.offlineAstroDataService = offlineAstroDataService;
    }

    [RelayCommand]
    private async Task OnSelectedTimeChangedAsync()
    {
        if (lastSelectedTime != SelectedTime)
        {
            lastSelectedTime = SelectedTime;
            await CalculateAsync();
        }
    }

    [RelayCommand]
    private async Task CalculateAsync()
    {
        DateTime date = SelectedDate.Date.Add(SelectedTime);

        ServiceResponse<MoonData?> offlineResult = await offlineAstroDataService.GetMoonDataAsync(date, 52.23, 21.01);

        if (offlineResult.IsSuccess && offlineResult.Data is not null)
            DisplayResoult(offlineResult.Data);
        //else display error message

        try
        {
            ServiceResponse<MoonData?> onlineResult = await onlineAstroDataService.GetMoonDataAsync(date, 52.23, 21.01);

            if (onlineResult.IsSuccess && onlineResult.Data is not null)
            {
                DisplayResoult(onlineResult.Data);
                return;
            }
        }
        catch (Exception ex)
        {
            if (ex is IndexOutOfRangeException or ArgumentNullException or JsonException or HttpRequestException or IOException)
                Debug.Write(ex); //display error
            else
                throw;
        }
    }

    public void DisplayResoult(MoonData data)
    {
        MoonPhaseName = data.Phase;
        MoonImage = MoonImages[data.Phase];
        SetIlluminationPerc(Math.Round(data.Illumination));
        SetMoonAge(Math.Round(data.MoonAge, 2));

        MoonriseDate = data.Rise.ToLocalTime().ToString("d MMM HH:mm");
        MoonsetDate = data.Set.ToLocalTime().ToString("d MMM HH:mm");
    }

    private void SetIlluminationPerc<T>(T value) => IlluminationPerc = $"{value}%";

    private void SetMoonAge(double value) => MoonAge = value < 2 ? $"{value} day" : $"{value} days";
}