using CommunityToolkit.Maui.Views;
using Photography_Tools.Components.Popups;
using Photography_Tools.Services.KeyValueStoreService;
using System.Collections.Frozen;
using System.Text.Json;

namespace Photography_Tools.ViewModels;

public partial class MoonPhaseViewModel : SaveableViewModel
{
    private static readonly FrozenDictionary<string, string> MoonImagesNorth, MoonImagesSouth;

    private readonly IAstroDataService onlineAstroDataService;
    private readonly IAstroDataService offlineAstroDataService;
    private readonly IKeyValueStore<Place> locationsKeyValueStore;
    private readonly IUiMessageService messageService;

    private TimeSpan lastSelectedTime = TimeSpan.Zero;

    [ObservableProperty]
    private string locationName = string.Empty;

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
        moonsetDate = string.Empty,
        dataSourceInfo = string.Empty;

    public bool UseOnlineAstroData { get; private set; } = true;

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

    public MoonPhaseViewModel([FromKeyedServices(KeyedServiceNames.OnlineAstroData)] IAstroDataService onlineAstroDataService, [FromKeyedServices(KeyedServiceNames.OfflineAstroData)] IAstroDataService offlineAstroDataService,
        IKeyValueStore<Place> locationsKeyValueStore, IPreferencesService preferencesService, IUiMessageService messageService) : base(preferencesService)
    {
        this.onlineAstroDataService = onlineAstroDataService;
        this.offlineAstroDataService = offlineAstroDataService;
        this.locationsKeyValueStore = locationsKeyValueStore;
        this.messageService = messageService;

        LocationName = preferencesService.GetPreference(PreferencesKeys.MoonPhaseUserInputPreferencesKey, string.Empty) ?? string.Empty;
    }

    [RelayCommand]
    protected void OnAppearing()
    {
#if DEBUG
        UseOnlineAstroData = preferencesService.GetPreference(PreferencesKeys.UseOnlineAstroDataPreferencesKey, true);
#else
        UseOnlineAstroData = preferencesService.GetPreference(PreferencesKeys.UseOnlineAstroDataPreferencesKey, true);
#endif
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
        Place? location = await locationsKeyValueStore.GetValueAsync(LocationName);

        if (location is null)
        {
            await messageService.ShowMessageAsync("No location selected", "Select a location to get results", "Ok");
            return;
        }

        GeographicalCoordinates coordinates = location.Coordinates;
        DateTime date = SelectedDate.Date.Add(SelectedTime);

        ServiceResponse<MoonData?> offlineResult = await offlineAstroDataService.GetMoonDataAsync(date, coordinates.Latitude, coordinates.Longitude);

        if (offlineResult.IsSuccess && offlineResult.Data is not null)
            DisplayResult(offlineResult.Data, coordinates.Latitude, offlineAstroDataService.DataSourceInfo);

        if (!UseOnlineAstroData)
            return;

        try
        {
            ServiceResponse<MoonData?> onlineResult = await onlineAstroDataService.GetMoonDataAsync(date, coordinates.Latitude, coordinates.Longitude);

            if (onlineResult.IsSuccess && onlineResult.Data is not null)
            {
                DisplayResult(onlineResult.Data, coordinates.Latitude, onlineAstroDataService.DataSourceInfo);
                return;
            }
            else
            {
                if (onlineResult.Code == -2)
                    return;

                await messageService.ShowMessageAsync("Online data source is not avaliable", $"{onlineResult.Message}\nLower accuracy data is only avaliable" ?? "Unexpected error occured\nOnly lower accuracy data is available", "Ok");
            }
        }
        catch (Exception ex)
        {
            if (ex is IndexOutOfRangeException or ArgumentNullException or JsonException or HttpRequestException or IOException)
                await messageService.ShowMessageAsync("Online data source is not avaliable", $"{ex.Message}\nOnly lower accuracy data is available", "Ok");
            else
                throw;
        }
    }

    [RelayCommand]
    private async Task ChangeLocationAsync()
    {
        if (Application.Current is null || Application.Current.MainPage is null)
            return;

        Place? selectedPlace = await Application.Current.MainPage.ShowPopupAsync(new LocationPopup(locationsKeyValueStore, messageService, LocationName)) as Place;

        if (selectedPlace is not null)
        {
            LocationName = selectedPlace.Name;
            await CalculateAsync();
        }
    }

    public void DisplayResult(MoonData data, double latitude, string sourceInfo)
    {
        MoonPhaseName = data.Phase;
        SetMoonImage(data.Phase, latitude);
        SetIlluminationPerc(Math.Round(data.Illumination));
        SetMoonAge(Math.Round(data.MoonAge, 2));
        SetDataSourceInfo(sourceInfo);

        MoonriseDate = data?.Rise?.ToLocalTime().ToString("d MMM HH:mm") ?? " --- ";
        MoonsetDate = data?.Set?.ToLocalTime().ToString("d MMM HH:mm") ?? " --- ";
    }

    public void SetMoonImage(string phaseName, double latitude) =>
        MoonImage = latitude >= 0 ? MoonImagesNorth[phaseName] : MoonImagesSouth[phaseName];

    public void SetIlluminationPerc<T>(T value) =>
        IlluminationPerc = $"{value}%";

    public void SetMoonAge(double value) =>
        MoonAge = value < 2 ? $"{value} day" : $"{value} days";

    public void SetDataSourceInfo(string info) =>
        DataSourceInfo = $"Source: {info}";

    protected override void SaveUserInput()
    {
        if (!string.IsNullOrWhiteSpace(LocationName) && LocationName.Length <= 127)
            preferencesService.SetPreference(PreferencesKeys.MoonPhaseUserInputPreferencesKey, LocationName);
    }
}