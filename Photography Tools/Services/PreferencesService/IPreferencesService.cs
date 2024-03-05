namespace Photography_Tools.Services.PreferencesService;

public interface IPreferencesService
{
    T? GetPreference<T>(string key, T? defaultValue = default);
    bool SetPreference<T>(string key, T value);
}