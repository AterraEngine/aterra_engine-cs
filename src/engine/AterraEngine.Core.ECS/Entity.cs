// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Core.Types;

namespace AterraEngine.Core.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Entity(EngineAssetId id, string? internalName=null)  : EngineAsset(id, internalName), IEntity {
    protected Dictionary<Type, IComponent> _components { get; } = new();
    public IReadOnlyDictionary<Type, IComponent> Components => _components.AsReadOnly();

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

    public bool CheckForComponent(Type t) {
        return _components.ContainsKey(t);
    }

    public bool TryAddComponent<T, T2>() where T : IComponent where T2 : T, IComponent, new() {
        return _components.TryAdd(typeof(T), new T2());
    }

    public bool TryRemoveComponent<T>() where T : IComponent {
        return _components.Remove(typeof(T));
    }

    public T GetComponent<T>() where T : IComponent {
        return (T)_components.GetValueOrDefault(typeof(T))! ?? throw new InvalidOperationException();
    }
}