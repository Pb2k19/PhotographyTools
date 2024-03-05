namespace Photography_Tools.Services.PreferencesService;

public class PreferencesService : IPreferencesService
{
    public T? GetPreference<T>(string key, T? defaultValue = default)
    {
        return Preferences.Default.Get(key, defaultValue);
    }

    public bool SetPreference<T>(string key, T value)
    {
        if (value is null)
            return false;

        if (value.Equals(GetPreference<T>(key)))
            return true;

        Preferences.Default.Set(key, value);
        return true;
    }
}