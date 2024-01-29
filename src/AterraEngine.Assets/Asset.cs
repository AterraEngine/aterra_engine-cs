// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Components;
using AterraEngine.Types;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Asset(EngineAssetId id, string? internalName=null) : EngineAsset(id, internalName) , IAsset {
    protected Dictionary<Type, IComponent> _components { get; set; } = new();
    public IReadOnlyDictionary<Type, IComponent> Components => _components.AsReadOnly();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T: IComponent {
        if (_components.TryGetValue(typeof(T), out IComponent? comp)) {
            component = (T)comp;
            return true;
        }

        component = default;
        return false;
    }

    public bool TryAddComponent<T, T2>() where T : IComponent where T2 : T, IComponent, new(){
        return  _components.TryAdd(typeof(T), new T2());
    }

    public bool TryRemoveComponent<T>() where T : IComponent {
        return _components.Remove(typeof(T));
    }

    protected T GetComponent<T>() where T : IComponent {
        if (!_components.TryGetValue(typeof(T), out IComponent? comp)) throw new Exception();
        return (T)comp;
    }
}