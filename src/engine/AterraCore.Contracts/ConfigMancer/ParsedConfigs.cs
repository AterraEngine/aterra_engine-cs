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
public readonly struct ParsedConfigs(IEnumerable<KeyValuePair<Type, object>> parsedConfig) : IParsedConfigs{
    // Expands the original dictionary to include their interfaces as well.
    private readonly ConcurrentDictionary<Type, object> _parsedConfig = new(parsedConfig
        .SelectMany(pair => pair.Key.GetInterfaces()
            .Where(t => typeof(IConfigMancerElement).IsAssignableFrom(t) && t != typeof(IConfigMancerElement))
            .Select(t => new KeyValuePair<Type, object>(t, pair.Value))
            .Append(pair)// Don't forget to include the original pair as well
        ));
    
    public int Count => _parsedConfig.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public ParsedConfigs() : this(new Dictionary<Type, object>()) {}
    public static ParsedConfigs Empty => new();

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
    
    public bool TryAddConfig<T>(T value) where T : class => _parsedConfig.TryAdd(typeof(T), value);
    public void AddOrUpdateConfig<T>(T value) where T : class => _parsedConfig.AddOrUpdate(typeof(T), value, (_, _) => value);
    public void AddOrUpdateConfig<T>(Type key, T value) where T : class => _parsedConfig.AddOrUpdate(key, value, (_, _) => value);

    public bool TryGetAndUpdate<T>(Type key, Func<T, bool> callback) {
        return _parsedConfig.TryGetValue(key, out object? obj) 
               && obj is T casted 
               && callback(casted) 
               && _parsedConfig.TryUpdate(key, casted, obj);
    }
    
}
