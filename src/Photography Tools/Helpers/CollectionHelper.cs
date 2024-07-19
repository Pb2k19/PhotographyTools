namespace Photography_Tools.Helpers;

public static class CollectionHelper
{
    public static TValue? GetValueOrNull<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) where TValue : struct
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        return dictionary.TryGetValue(key, out TValue value) ? value : null;
    }
}