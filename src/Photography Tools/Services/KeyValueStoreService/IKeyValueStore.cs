using System.Collections.Immutable;

namespace Photography_Tools.Services.KeyValueStoreService;

public interface IKeyValueStore<T> where T : class
{
    string FileName { get; }
    string FilePath { get; }

    ValueTask<T?> GetValueAsync(string key);
    Task<bool> AddAsync(string key, T value);
    Task<bool> AddOrUpdateAsync(string key, T value);
    Task<bool> RemoveAsync(string key);
    ValueTask<ImmutableArray<string>> GetAllKeys();
}