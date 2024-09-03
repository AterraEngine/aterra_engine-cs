// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.ConfigMancer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct ParsedConfigs(IDictionary<Type, object> parsedConfig) {
    // Expands the original dictionary to include their interfaces as well.
    private readonly FrozenDictionary<Type, object> _parsedConfig = parsedConfig
        .SelectMany(pair => pair.Key.GetInterfaces()
            .Where(t => typeof(IConfigMancerElement).IsAssignableFrom(t) && t != typeof(IConfigMancerElement))
            .Select(t => new KeyValuePair<Type, object>(t, pair.Value))
            .Append(pair) // Don't forget to include the original pair as well
        )
        .ToFrozenDictionary();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public ParsedConfigs() : this(new Dictionary<Type, object>()) { }
    public static readonly ParsedConfigs Empty = new();

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
    
    public int Count => _parsedConfig.Keys.Length;
}
