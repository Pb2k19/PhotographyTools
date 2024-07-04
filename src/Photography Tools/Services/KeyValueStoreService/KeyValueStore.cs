using System.Collections.Immutable;
using System.Text.Json;

namespace Photography_Tools.Services.KeyValueStoreService;

public class KeyValueStore<T> : IKeyValueStore<T> where T : class
{
    private const int SempahoreTimeout = 5000;

    private readonly SemaphoreSlim semaphore = new(1);

    private Dictionary<string, T> dictionary = [];

    public string FileName { get; }

    public string FolderPath { get; }

    public string FilePath { get; }

    public KeyValueStore(string fileName, string folderPath)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentNullException(nameof(fileName));
        else if (string.IsNullOrWhiteSpace(folderPath))
            throw new ArgumentNullException(nameof(folderPath));

        FileName = fileName;
        FolderPath = folderPath;
        FilePath = Path.Combine(folderPath, fileName);
    }

    public async ValueTask<T?> GetValueAsync(string key)
    {
        if (dictionary.Count == 0)
        {
            if (await semaphore.WaitAsync(SempahoreTimeout))
            {
                try
                {
                    if (!await LoadDataAsync())
                        return default;
                }
                catch (Exception ex)
                {
                    if (ex is IOException or UnauthorizedAccessException or JsonException)
                        return default;

                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
            }
            else
                return default;
        }

        return dictionary.TryGetValue(key, out T? value) ? value : default;
    }

    public async ValueTask<ImmutableArray<string>> GetAllKeys()
    {
        if (dictionary.Count == 0)
        {
            if (await semaphore.WaitAsync(SempahoreTimeout))
            {
                try
                {
                    if (!await LoadDataAsync())
                        return [];
                }
                catch (Exception ex)
                {
                    if (ex is IOException or UnauthorizedAccessException or JsonException)
                        return [];

                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
            }
            else
                return [];
        }

        return [.. dictionary.Keys];
    }

    public async Task<bool> AddAsync(string key, T value)
    {
        if (!await semaphore.WaitAsync(SempahoreTimeout))
            return false;

        try
        {
            if (!dictionary.TryAdd(key, value))
                return false;

            await SaveToFileAsync();
        }
        catch (Exception ex)
        {
            dictionary.Remove(key);

            if (ex is IOException or UnauthorizedAccessException or JsonException)
                return false;

            throw;
        }
        finally
        {
            semaphore.Release();
        }

        return true;
    }

    public async Task<bool> RemoveAsync(string key)
    {
        if (!await semaphore.WaitAsync(SempahoreTimeout))
            return false;

        T? oldValue = null;
        try
        {
            oldValue = await GetValueAsync(key);

            if (!dictionary.Remove(key))
                return false;

            await SaveToFileAsync();
        }
        catch (Exception ex)
        {
            if (oldValue is not null)
                dictionary.TryAdd(key, oldValue);

            if (ex is IOException or UnauthorizedAccessException or JsonException)
                return false;

            throw;
        }
        finally
        {
            semaphore.Release();
        }

        return true;
    }

    public async Task<bool> AddOrUpdateAsync(string key, T value)
    {
        if (!await semaphore.WaitAsync(SempahoreTimeout))
            return false;

        T? oldValue = null;
        bool isUpdate = false;
        try
        {
            isUpdate = dictionary.TryGetValue(key, out oldValue);

            if (oldValue is not null && oldValue.Equals(value))
                return false;

            dictionary[key] = value;
            await SaveToFileAsync();
        }
        catch (Exception ex)
        {
            if (isUpdate && oldValue is not null)
                dictionary[key] = oldValue;
            else
                dictionary.Remove(key);

            if (ex is IOException or UnauthorizedAccessException or JsonException)
                return false;

            throw;
        }
        finally
        {
            semaphore.Release();
        }

        return true;
    }

    private async Task<bool> LoadDataAsync()
    {
        if (!File.Exists(FilePath))
            return false;

        using FileStream fs = File.OpenRead(FilePath);
        dictionary = (await JsonSerializer.DeserializeAsync<Dictionary<string, T>>(fs)) ?? [];
        return dictionary.Count > 0;
    }

    private async Task SaveToFileAsync()
    {
        Directory.CreateDirectory(FolderPath);
        using FileStream fileStream = new(FilePath, FileMode.Create);
        await JsonSerializer.SerializeAsync(fileStream, dictionary);
    }
}