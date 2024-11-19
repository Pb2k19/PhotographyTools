using CommunityToolkit.Maui.Views;
using Photography_Tools.Components.Popups;
using Photography_Tools.Services.KeyValueStoreService;

namespace Photography_Tools.ViewModels;

public abstract partial class AstroLocationViewModel : SaveableViewModel
{
    protected readonly IAstroDataService onlineAstroDataService;
    protected readonly IAstroDataService offlineAstroDataService;
    protected readonly IKeyValueStore<Place> locationsKeyValueStore;
    protected readonly IUiMessageService messageService;

    [ObservableProperty]
    private string locationName = string.Empty;

    [ObservableProperty]
    private DateTime selectedDate;

    [ObservableProperty]
    private string dataSourceInfo = string.Empty;

    public bool UseOnlineService { get; protected set; } = true;

    protected AstroLocationViewModel([FromKeyedServices(KeyedServiceNames.OnlineAstroData)] IAstroDataService onlineAstroDataService, [FromKeyedServices(KeyedServiceNames.OfflineAstroData)] IAstroDataService offlineAstroDataService,
        IKeyValueStore<Place> locationsKeyValueStore, IPreferencesService preferencesService, IUiMessageService messageService) : base(preferencesService)
    {
        this.onlineAstroDataService = onlineAstroDataService;
        this.offlineAstroDataService = offlineAstroDataService;
        this.locationsKeyValueStore = locationsKeyValueStore;
        this.messageService = messageService;
    }

    protected abstract Task CalculateAsync();

    [RelayCommand]
    private async Task ChangeLocationAsync()
    {
        Page? mainPage = UiHelper.GetMainPage();
        if (mainPage is null)
            return;

        Place? selectedPlace = await mainPage.ShowPopupAsync(new LocationPopup(locationsKeyValueStore, messageService, LocationName)) as Place;

        if (selectedPlace is not null)
        {
            LocationName = selectedPlace.Name;
            await CalculateAsync();
        }
    }

    public void SetDataSourceInfo(string info) =>
        DataSourceInfo = $"Source: {info}";
}
