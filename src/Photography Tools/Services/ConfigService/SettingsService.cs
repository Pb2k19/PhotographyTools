
namespace Photography_Tools.Services.ConfigService;

public class SettingsService : ISettingsService
{
    private readonly IPreferencesService preferencesService;

    Settings currentSettings;

    public SettingsService(IPreferencesService preferencesService)
    {
        this.preferencesService = preferencesService;
        currentSettings = preferencesService.GetDeserailizedPreference<Settings>(PreferencesKeys.SettingsKey) ?? ISettingsService.DefaultSettings;
    }

    public Settings GetSettings() => currentSettings;

    public int UpdateSettings(Settings newSettings)
    {
        if (currentSettings.Equals(newSettings))
            return 0;

        bool result = preferencesService.SerializeAndSetPreference(PreferencesKeys.SettingsKey, newSettings);

        if (!result)
            return -1;

        currentSettings = newSettings;
        return 1;
    }
}
