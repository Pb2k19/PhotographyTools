﻿using System.Text.Json;

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

    public bool SerializedAndSetPreference<T>(string preferenceKey, T obj) where T : class
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

        return JsonSerializer.Deserialize<T>(serialized);
    }
}