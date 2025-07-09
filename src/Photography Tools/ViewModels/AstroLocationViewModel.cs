using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Photography_Tools.Components.Popups;
using Photography_Tools.Services.KeyValueStoreService;

namespace Photography_Tools.ViewModels;

public abstract partial class AstroLocationViewModel : SaveableViewModel
{
    protected readonly Dictionary<string, object> locationDictionary = [];
    protected readonly IAstroDataService onlineAstroDataService;
    protected readonly IAstroDataService offlineAstroDataService;
    protected readonly IKeyValueStore<Place> locationsKeyValueStore;
    protected readonly IUiMessageService messageService;
    protected readonly IPopupService popupService;

    [ObservableProperty]
    private string locationName = string.Empty;

    [ObservableProperty]
    private DateTime selectedDate;

    [ObservableProperty]
    private string dataSourceInfo = string.Empty;

    public bool UseOnlineService { get; protected set; } = true;

    public bool IsPopupPresented { get; protected set; } = false;

    protected AstroLocationViewModel([FromKeyedServices(KeyedServiceNames.OnlineAstroData)] IAstroDataService onlineAstroDataService, [FromKeyedServices(KeyedServiceNames.OfflineAstroData)] IAstroDataService offlineAstroDataService,
        IKeyValueStore<Place> locationsKeyValueStore, IPreferencesService preferencesService, IUiMessageService messageService, IPopupService popupService) : base(preferencesService)
    {
        this.onlineAstroDataService = onlineAstroDataService;
        this.offlineAstroDataService = offlineAstroDataService;
        this.locationsKeyValueStore = locationsKeyValueStore;
        this.messageService = messageService;
        this.popupService = popupService;
    }

    protected abstract Task CalculateAsync();

    [RelayCommand]
    private async Task ChangeLocationAsync()
    {
#if WINDOWS
        //tmp
        MainWindow? mainWindow = Application.Current?.Windows.Count > 0 ? Application.Current?.Windows[0] as MainWindow : null;
        mainWindow?.SetTitleBarNull();
#endif

        locationDictionary[nameof(LocationPopup.InitSelected)] = LocationName;
        IsPopupPresented = true;
        try
        {
            IPopupResult<Place> popupResult = await popupService.ShowPopupAsync<LocationPopup, Place>(Shell.Current);

            if (popupResult.WasDismissedByTappingOutsideOfPopup || popupResult.Result is null)
                return;

            LocationName = popupResult.Result.Name;
        }
        finally
        {
#if WINDOWS
            //tmp
            mainWindow?.ShowTitleBar();
#endif
            IsPopupPresented = false;
        }

        await CalculateAsync();
    }

    public void SetDataSourceInfo(string info) =>
        DataSourceInfo = $"Source: {info}";
}
