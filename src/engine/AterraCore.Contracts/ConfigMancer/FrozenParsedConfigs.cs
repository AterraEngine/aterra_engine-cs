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
public readonly struct FrozenParsedConfigs(IEnumerable<KeyValuePair<Type, IConfigMancerElement>> parsedConfig) : IParsedConfigs {
    // Expands the original dictionary to include their interfaces as well.
    private readonly FrozenDictionary<Type, IConfigMancerElement> _parsedConfig = parsedConfig
        .SelectMany(
            pair => pair.Key.GetInterfaces()
                .Select(t => new KeyValuePair<Type, IConfigMancerElement>(t, pair.Value))
                .Append(pair)// Don't forget to include the original pair as well
        )
        .ToFrozenDictionary();

    public int Count => _parsedConfig.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public FrozenParsedConfigs() : this(new Dictionary<Type, IConfigMancerElement>()) {}
    public static FrozenParsedConfigs Empty => new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetConfig<T>([NotNullWhen(true)] out T? value) where T : IConfigMancerElement {
        value = default;
        Type type = typeof(T);

        if (!_parsedConfig.TryGetValue(type, out IConfigMancerElement? obj) || obj is not T casted) return false;

        value = casted;
        return true;
    }
    public IReadOnlyDictionary<Type, IConfigMancerElement> AsReadOnlyDictionary() => _parsedConfig.AsReadOnly();

}
