namespace Photography_Tools.Services.KeyValueStoreService;

public static class KeyValueStoreFactory
{
    public static IKeyValueStore<AstroData> CreateCacheAstroDataKeyValueStore() =>
        new KeyValueStore<AstroData>("Cache.json", Path.Combine(FileSystem.Current.CacheDirectory, "AppCache"));

    public static IKeyValueStore<Place> CreateLocationKeyValueStore() =>
        new KeyValueStore<Place>("Locations.json", Path.Combine(FileSystem.Current.AppDataDirectory, "Locations"));
}