using System.Collections.Frozen;
using System.Diagnostics;
using System.Text.Json;

namespace Photography_Tools.ViewModels;

public partial class MoonPhaseViewModel : ObservableObject
{
    private static readonly FrozenDictionary<string, string> MoonImagesNorth, MoonImagesSouth;

    private readonly IAstroDataService onlineAstroDataService;
    private readonly IAstroDataService offlineAstroDataService;
    private TimeSpan lastSelectedTime = TimeSpan.Zero;

    [ObservableProperty]
    private Place place = new("Warsaw", new(-52.23, 21.01));

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

    static MoonPhaseViewModel()
    {
        MoonImagesNorth = new Dictionary<string, string>
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

        MoonImagesSouth = new Dictionary<string, string>
        {
            {"New Moon", "🌑"},
            {"Waxing Crescent", "🌘"},
            {"First Quarter", "🌗"},
            {"Waxing Gibbous", "🌖"},
            {"Full Moon", "🌕"},
            {"Waning Gibbous", "🌔"},
            {"Third Quarter", "🌓"},
            {"Waning Crescent", "🌒"},
        }.ToFrozenDictionary();
    }

    public MoonPhaseViewModel([FromKeyedServices(KeyedServiceNames.OnlineAstroData)] IAstroDataService onlineAstroDataService, [FromKeyedServices(KeyedServiceNames.OfflineAstroData)] IAstroDataService offlineAstroDataService, IUiMessageService messageService)
    {
        this.onlineAstroDataService = onlineAstroDataService;
        this.offlineAstroDataService = offlineAstroDataService;
        this.messageService = messageService;
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
        GeographicalCoordinates coordinates = Place.Coordinates;

        ServiceResponse<MoonData?> offlineResult = await offlineAstroDataService.GetMoonDataAsync(date, coordinates.Latitude, coordinates.Longitude);

        if (offlineResult.IsSuccess && offlineResult.Data is not null)
            DisplayResult(offlineResult.Data, coordinates.Latitude);
        //else display error message

        try
        {
            ServiceResponse<MoonData?> onlineResult = await onlineAstroDataService.GetMoonDataAsync(date, coordinates.Latitude, coordinates.Longitude);

            if (onlineResult.IsSuccess && onlineResult.Data is not null)
            {
                DisplayResult(onlineResult.Data, coordinates.Latitude);
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

    public void DisplayResult(MoonData data, double latitude)
    {
        MoonPhaseName = data.Phase;
        SetMoonImage(data.Phase, latitude);
        SetIlluminationPerc(Math.Round(data.Illumination));
        SetMoonAge(Math.Round(data.MoonAge, 2));

        MoonriseDate = data.Rise.ToLocalTime().ToString("d MMM HH:mm");
        MoonsetDate = data.Set.ToLocalTime().ToString("d MMM HH:mm");
    }

    public void SetMoonImage(string phaseName, double latitude)
    {
        MoonImage = latitude >= 0 ? MoonImagesNorth[phaseName] : MoonImagesSouth[phaseName];
    }

    public void SetIlluminationPerc<T>(T value) => IlluminationPerc = $"{value}%";

    public void SetMoonAge(double value) => MoonAge = value < 2 ? $"{value} day" : $"{value} days";
}