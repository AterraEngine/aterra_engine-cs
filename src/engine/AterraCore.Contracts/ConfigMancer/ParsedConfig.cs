// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using JetBrains.Annotations;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.ConfigMancer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct ParsedConfigs(IDictionary<AssetId, object> parsedConfig) {
    private readonly ImmutableDictionary<AssetId, object> _parsedConfig = parsedConfig.ToImmutableDictionary();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public ParsedConfigs() : this(new Dictionary<AssetId, object>()) { }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetConfig<T>(AssetId key, [NotNullWhen(true)] out T? value) where T : class {
        value = null;
        if (!_parsedConfig.TryGetValue(key, out object? obj) || obj is not T casted) return false;
        value = casted;
        return true;
    }
    
    public int Count() {
        return _parsedConfig.Keys.Count();
    }
}
