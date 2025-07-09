namespace Photography_Tools.Services.PreferencesService;

public interface IPreferencesService
{
    void ClearAll();
    T? GetDeserailizedPreference<T>(string preferenceKey, T? defaultValue = null) where T : class;
    T? GetPreference<T>(string key, T? defaultValue = default);
    bool SerializeAndSetPreference<T>(string preferenceKey, T obj) where T : class;
    bool SetPreference<T>(string key, T value);
}