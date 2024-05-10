using System.Text.Json;

namespace Photography_Tools.Services.KeyValueStoreService;

public class CacheKeyValueStore<T> : IKeyValueStore<T> where T : class
{
    private const int SempahoreTimeout = 5000;
    private const string FileNameConst = "Cache.json", FolderName = "AppCache";

    private readonly SemaphoreSlim semaphore = new(1);
    private readonly string
        folderPath = Path.Combine(FileSystem.Current.CacheDirectory, FolderName),
        filePath = Path.Combine(FileSystem.Current.CacheDirectory, FolderName, FileNameConst);

    private Dictionary<string, T> dictionary = [];

    public string FileName => FileNameConst;

    public string FilePath => filePath;

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
        catch (Exception)
        {
            dictionary.Remove(key);
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

        try
        {
            if (!dictionary.Remove(key))
                return false;

            await SaveToFileAsync();
        }
        catch (Exception)
        {
            dictionary.Remove(key);
            throw;
        }
        finally
        {
            semaphore.Release();
        }

        return true;
    }

    public async Task<bool> UpdateAsync(string key, T value)
    {
        if (!await semaphore.WaitAsync(SempahoreTimeout))
            return false;

        try
        {
            if (!dictionary.ContainsKey(key))
                return false;

            dictionary[key] = value;
            await SaveToFileAsync();
        }
        catch (Exception)
        {
            dictionary.Remove(key);
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
        Directory.CreateDirectory(folderPath);
        using FileStream fileStream = new(filePath, FileMode.Create);
        await JsonSerializer.SerializeAsync(fileStream, dictionary);
    }
}