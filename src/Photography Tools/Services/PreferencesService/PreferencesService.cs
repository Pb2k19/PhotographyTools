using System.Text.Json;

namespace Photography_Tools.Services.PreferencesService;

public class PreferencesService : IPreferencesService
{
    private readonly IPreferences preferences;

    public PreferencesService(IPreferences preferences)
    {
        this.preferences = preferences;
    }

    public void ClearAll()
    {
        preferences.Clear();
    }

    public T? GetPreference<T>(string key, T? defaultValue = default)
    {
        return preferences.Get(key, defaultValue);
    }

    public bool SetPreference<T>(string key, T value)
    {
        if (value is null)
            return false;

        if (value.Equals(GetPreference<T>(key)))
            return true;

        preferences.Set(key, value);
        return true;
    }

    public bool SerializeAndSetPreference<T>(string preferenceKey, T obj) where T : class
    {
        string serialized = JsonSerializer.Serialize(obj);
        SetPreference(preferenceKey, serialized);
        return true;
    }

    public T? GetDeserailizedPreference<T>(string preferenceKey, T? defaultValue = default) where T : class
    {
        string? serialized = GetPreference(preferenceKey, string.Empty);

        if (string.IsNullOrWhiteSpace(serialized))
            return defaultValue;

        try
        {
            return JsonSerializer.Deserialize<T>(serialized);
        }
        catch (JsonException)
        {
            return defaultValue;
        }
    }
}