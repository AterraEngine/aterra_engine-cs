// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Core.Assets;
using AterraEngine.Contracts.Core.ECSFramework;
using AterraEngine.Core.Assets;
using AterraEngine.Core.ServicesFramework;
using AterraEngine.Core.Types;
using Serilog;

namespace AterraEngine.Core.ECSFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Entity<TDto>(IAssetAtlas assetAtlas, ILogger logger) : Asset<TDto>(logger), IEntity where TDto : class {
    private readonly ConcurrentDictionary<Type, IComponent> _components = new();
    public ReadOnlyDictionary<Type, IComponent> Components => _components.AsReadOnly();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T : IComponent {
        if (_components.TryGetValue(typeof(T), out IComponent? comp)) {
            component = (T)comp;
            return true;
        }

        component = default;
        return false;
    }

    public bool HasComponent<T>() where T : IComponent {
        return _components.ContainsKey(typeof(T));
    }
    
    public bool TryAddComponent<TInterface>(AssetId id) where TInterface : class, IComponent {
        return assetAtlas.TryGetAsset(id, out IComponent? asset)
               && _components.TryAdd(typeof(TInterface), asset);
    }

    public bool TryRemoveComponent<T>( [NotNullWhen(true)] out IComponent? component) where T : IComponent {
        return _components.Remove(typeof(T), out component);
    }

    protected T GetComponent<T>() where T : IComponent {
        return (T)_components.GetValueOrDefault(typeof(T))! ?? throw new InvalidOperationException();
    }
}