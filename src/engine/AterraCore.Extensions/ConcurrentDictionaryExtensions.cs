// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace AterraCore.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class ConcurrentDictionaryExtensions {
    /// <summary>
    /// Populates a ConcurrentDictionary with empty values for all keys of a given Enum type.
    /// </summary>
    /// <typeparam name="TKey">The enum type representing the keys of the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The ConcurrentDictionary to populate with empty values.</param>
    /// <returns>The same ConcurrentDictionary with empty values added for all keys of the Enum type.</returns>
    public static ConcurrentDictionary<TKey, TValue> PopulateWithEmpties<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary )
        where TValue : new()
        where TKey : Enum {
        
        foreach (TKey key in Enum.GetValues(typeof(TKey))) {
            dictionary.TryAdd(key, new TValue());
        }

        return dictionary;
    }

    /// <summary>
    /// Tries to add or update a value in the ConcurrentDictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The ConcurrentDictionary to add or update the value in.</param>
    /// <param name="key">The key to add or update the value for.</param>
    /// <param name="value">The value to add or update.</param>
    /// <returns>True if the value is added or updated successfully; otherwise, false.</returns>
    public static bool TryAddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, ConcurrentBag<TValue>> dictionary, TKey key, TValue value) where TKey : notnull {
        if (!dictionary.TryGetValue(key, out ConcurrentBag<TValue>? existingBag)) {
            return dictionary.TryAdd(key, [value]);
        }
        existingBag.Add(value);
        return true;
    }
}