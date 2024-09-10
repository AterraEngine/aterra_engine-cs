// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.ConfigMancer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct ParsedConfigs(IEnumerable<KeyValuePair<Type, IConfigMancerElement>> parsedConfig) : IParsedConfigs {
    // Expands the original dictionary to include their interfaces as well.
    private readonly ConcurrentDictionary<Type, IConfigMancerElement> _parsedConfig = new(parsedConfig
        .SelectMany(pair => pair.Key.GetInterfaces()
            .Select(t => new KeyValuePair<Type, IConfigMancerElement>(t, pair.Value))
            .Append(pair)// Don't forget to include the original pair as well
        ));

    public int Count => _parsedConfig.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public ParsedConfigs() : this(new Dictionary<Type, IConfigMancerElement>()) {}
    public static ParsedConfigs Empty => new();

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

    public bool TryAddConfig<T>(T value) where T : IConfigMancerElement => _parsedConfig.TryAdd(typeof(T), value);
    public void AddOrUpdateConfig<T>(T value) where T : IConfigMancerElement => _parsedConfig.AddOrUpdate(typeof(T), value, updateValueFactory: (_, _) => value);
    public void AddOrUpdateConfig<T>(Type key, T value) where T : IConfigMancerElement => _parsedConfig.AddOrUpdate(key, value, updateValueFactory: (_, _) => value);
}
