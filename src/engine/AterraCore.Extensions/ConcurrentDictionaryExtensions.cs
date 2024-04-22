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
}