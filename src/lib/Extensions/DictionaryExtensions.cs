// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace Extensions;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public static class DictionaryExtensions {
    /// <summary>
    /// Populates a ConcurrentDictionary with empty values for all keys of a given Enum type.
    /// </summary>
    /// <typeparam name="TKey">The enum type representing the keys of the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <typeparam name="TDictionary">The type of the dictionary</typeparam>
    /// <param name="dictionary">The ConcurrentDictionary to populate with empty values.</param>
    /// <returns>
    /// The same ConcurrentDictionary with empty values added for all keys of the Enum type.
    /// </returns>
    [UsedImplicitly]
    public static TDictionary PopulateWithEmpties<TDictionary, TKey, TValue>(this TDictionary dictionary)
        where TDictionary : IDictionary<TKey, TValue>
        where TValue : new()
        where TKey : Enum {

        foreach (TKey key in Enum.GetValues(typeof(TKey))) {
            dictionary.TryAdd(key, new TValue());
        }

        return dictionary;
    }
}
