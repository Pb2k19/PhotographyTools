using Photography_Tools.Services.ConfigService;

namespace Photography_Tools.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IPreferencesService preferencesService;
    private readonly ISettingsService settingsService;
    private readonly IUiMessageService uiMessageService;

    private Settings currentSettings = ISettingsService.DefaultSettings;

    [ObservableProperty]
    private bool isUseOfflineDataSourceModeEnabled = false, isVibrationsEnabled = true, isVibrationsSupported = true;

    public SettingsViewModel(IPreferencesService preferencesService, ISettingsService settingsService, IUiMessageService uiMessageService)
    {
        this.preferencesService = preferencesService;
        this.settingsService = settingsService;
        this.uiMessageService = uiMessageService;
    }

    [RelayCommand]
    private void OnDisappearing()
    {
        Settings newSettings = currentSettings with
        {
            IsUseOnlyOfflineDataSourceModeEnabled = IsUseOfflineDataSourceModeEnabled,
            IsVibrationsEnabled = IsVibrationsEnabled
        };

        int result = settingsService.UpdateSettings(newSettings);

        if (result is 0)
            return;
        else if (result is -1)
        {
            SetValues(currentSettings);
            uiMessageService.ShowMessageAndForget("Something went wrong", "Configuration could not be saved - changes were cancelled", "Ok");
        }
    }

    [RelayCommand]
    private void OnAppearing()
    {
        SetValues(settingsService.GetSettings());
        IsVibrationsSupported = Vibration.Default.IsSupported;
    }

    [RelayCommand]
    private async Task ResetAllPreferencesAsync()
    {
        bool result = await uiMessageService.ShowMessageAsync("Are you shure about that?", """If you click "Ok" you will delete all saved preferences""", "Ok", "Cancel");

        if (!result)
            return;

        preferencesService.ClearAll();
        SetValues(ISettingsService.DefaultSettings);
    }

    [RelayCommand]
    private void ToggleOfflineDataOnly()
    {
        IsUseOfflineDataSourceModeEnabled = !IsUseOfflineDataSourceModeEnabled;
    }

    [RelayCommand]
    private void ToggleVibrations()
    {
        IsVibrationsEnabled = !IsVibrationsEnabled;
    }

    private void SetValues(Settings settings)
    {
        currentSettings = settings;
        IsUseOfflineDataSourceModeEnabled = settings.IsUseOnlyOfflineDataSourceModeEnabled;
        IsVibrationsEnabled = settings.IsVibrationsEnabled;
    }
}
