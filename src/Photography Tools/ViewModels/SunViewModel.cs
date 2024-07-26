using CommunityToolkit.Maui.Views;
using Photography_Tools.Components.Popups;
using Photography_Tools.Services.KeyValueStoreService;
using System.Text.Json;

namespace Photography_Tools.ViewModels;

public partial class SunViewModel : SaveableViewModel
{
    private readonly IAstroDataService onlineAstroDataService;
    private readonly IAstroDataService offlineAstroDataService;
    private readonly IKeyValueStore<Place> locationsKeyValueStore;
    private readonly IUiMessageService messageService;

    [ObservableProperty]
    private string locationName = string.Empty;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today.AddHours(12);

    [ObservableProperty]
    private string
        sunriseDate = string.Empty,
        sunsetDate = string.Empty,
        morningGoldenHourStartDate = string.Empty,
        morningGoldenHourEndDate = string.Empty,
        eveningGoldenHourStartDate = string.Empty,
        eveningGoldenHourEndDate = string.Empty,
        morningBlueHourStartDate = string.Empty,
        morningBlueHourEndDate = string.Empty,
        eveningBlueHourStartDate = string.Empty,
        eveningBlueHourEndDate = string.Empty,
        morningCivilTwilightStartDate = string.Empty,
        morningCivilTwilightEndDate = string.Empty,
        eveningCivilTwilightStartDate = string.Empty,
        eveningCivilTwilightEndDate = string.Empty,
        upperTransitDate = string.Empty,
        dataSourceInfo = string.Empty;

    public bool UseOnlineAstroData { get; private set; } = true;

    public SunViewModel([FromKeyedServices(KeyedServiceNames.OnlineAstroData)] IAstroDataService onlineAstroDataService, [FromKeyedServices(KeyedServiceNames.OfflineAstroData)] IAstroDataService offlineAstroDataService,
        IKeyValueStore<Place> locationsKeyValueStore, IPreferencesService preferencesService, IUiMessageService messageService) : base(preferencesService)
    {
        this.onlineAstroDataService = onlineAstroDataService;
        this.offlineAstroDataService = offlineAstroDataService;
        this.locationsKeyValueStore = locationsKeyValueStore;
        this.messageService = messageService;

        LocationName = preferencesService.GetPreference(PreferencesKeys.MoonPhaseUserInputPreferencesKey, string.Empty) ?? string.Empty;
    }

    [RelayCommand]
    protected async Task OnAppearingAsync()
    {
#if DEBUG
        UseOnlineAstroData = preferencesService.GetPreference(PreferencesKeys.UseOnlineAstroDataPreferencesKey, false);
#else
        UseOnlineAstroData = preferencesService.GetPreference(PreferencesKeys.UseOnlineAstroDataPreferencesKey, true);
#endif
        await CalculateAsync();
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
        DateTime date = SelectedDate.Date.AddHours(12);

        ServiceResponse<SunPhasesResult?> offlineResult = await offlineAstroDataService.GetSunDataAsync(date, coordinates.Latitude, coordinates.Longitude);

        if (offlineResult.IsSuccess && offlineResult.Data is not null)
            DisplayResult(offlineResult.Data, offlineAstroDataService.DataSourceInfo);

        if (!UseOnlineAstroData)
            return;

        try
        {
            ServiceResponse<SunPhasesResult?> onlineResult = await onlineAstroDataService.GetSunDataAsync(date, coordinates.Latitude, coordinates.Longitude);

            if (onlineResult.IsSuccess && onlineResult.Data is not null)
            {
                DisplayResult(onlineResult.Data, onlineAstroDataService.DataSourceInfo);
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

    public void DisplayResult(SunPhasesResult data, string sourceInfo)
    {
        SetDataSourceInfo(sourceInfo);

        SunriseDate = data.Day.StartDate.ToStringLocalTime();
        UpperTransitDate = data.UpperTransit.ToStringLocalTime();
        SunsetDate = data.Day.EndDate.ToStringLocalTime();

        MorningCivilTwilightStartDate = data.MorningCivilTwilight.StartDate.ToStringLocalTime();
        MorningCivilTwilightEndDate = data.MorningCivilTwilight.EndDate.ToStringLocalTime();

        MorningBlueHourStartDate = data.MorningBlueHour.StartDate.ToStringLocalTime();
        MorningBlueHourEndDate = data.MorningBlueHour.EndDate.ToStringLocalTime();

        MorningGoldenHourStartDate = data.MorningGoldenHour.StartDate.ToStringLocalTime();
        MorningGoldenHourEndDate = data.MorningGoldenHour.EndDate.ToStringLocalTime();

        EveningGoldenHourStartDate = data.EveningGoldenHour.StartDate.ToStringLocalTime();
        EveningGoldenHourEndDate = data.EveningGoldenHour.EndDate.ToStringLocalTime();

        EveningBlueHourStartDate = data.EveningBlueHour.StartDate.ToStringLocalTime();
        EveningBlueHourEndDate = data.EveningBlueHour.EndDate.ToStringLocalTime();

        EveningCivilTwilightStartDate = data.EveningCivilTwilight.StartDate.ToStringLocalTime();
        EveningCivilTwilightEndDate = data.EveningCivilTwilight.EndDate.ToStringLocalTime();
    }

    public void SetDataSourceInfo(string info) =>
        DataSourceInfo = $"Source: {info}";

    protected override void SaveUserInput()
    {
        if (!string.IsNullOrWhiteSpace(LocationName) && LocationName.Length <= 127)
            preferencesService.SetPreference(PreferencesKeys.SunUserInputPreferencesKey, LocationName);
    }
}