// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.ConfigMancer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct FrozenParsedConfigs(IEnumerable<KeyValuePair<Type, object>> parsedConfig) : IParsedConfigs {
    // Expands the original dictionary to include their interfaces as well.
    private readonly FrozenDictionary<Type, object> _parsedConfig = parsedConfig
        .SelectMany(pair => pair.Key.GetInterfaces()
                .Where(t => typeof(IConfigMancerElement).IsAssignableFrom(t) && t != typeof(IConfigMancerElement))
                .Select(t => new KeyValuePair<Type, object>(t, pair.Value))
                .Append(pair)// Don't forget to include the original pair as well
        )
        .ToFrozenDictionary();

    public int Count => _parsedConfig.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public FrozenParsedConfigs() : this(new Dictionary<Type, object>()) {}
    public static FrozenParsedConfigs Empty => new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetConfig<T>([NotNullWhen(true)] out T? value) where T : class {
        value = null;
        Type type = typeof(T);

        if (!_parsedConfig.TryGetValue(type, out object? obj) || obj is not T casted) return false;

        value = casted;
        return true;
    }
    public IReadOnlyDictionary<Type, object> AsReadOnlyDictionary() => _parsedConfig.AsReadOnly();

}
