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
public readonly struct ParsedConfigs(IDictionary<AssetId, dynamic> parsedConfig) {
    private readonly ImmutableDictionary<AssetId, dynamic> _parsedConfig = parsedConfig.ToImmutableDictionary();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public ParsedConfigs() : this(new Dictionary<AssetId, dynamic>()) { }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetConfig<T>(AssetId key, [NotNullWhen(true)] out T? value) where T : class {
        value = null;

        if (!_parsedConfig.TryGetValue(key, out dynamic? o)) {
            Console.WriteLine($"Config for key {key} not found.");
            return false;
        }

        try {
            value = o;
            return true;
        }
        catch (Exception e) {
            Console.WriteLine($"Config for key {key} not found.");
            Console.WriteLine(e);
            return false;
        }
    }
    
    public int Count() {
        return _parsedConfig.Keys.Count();
    }
}
