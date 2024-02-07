// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.ECS;
using OldAterraEngine.Core.Types;

namespace OldAterraEngine.Core.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Entity(EngineAssetId id, string? internalName=null)  : EngineAsset(id, internalName), IEntity {
    private ConcurrentDictionary<Type, IComponent> _components { get; } = new();
    public ReadOnlyDictionary<Type, IComponent> Components => _components.AsReadOnly();

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

    public bool TryAddComponent<T, T2>() where T : IComponent where T2 : T, IComponent {
        return _components.TryAdd(typeof(T), EngineServices.CreateWithServices<T2>());
    }

    public bool TryRemoveComponent<T>( [NotNullWhen(true)] out IComponent? component) where T : IComponent {
        return _components.Remove(typeof(T), out component);
    }

    public T GetComponent<T>() where T : IComponent {
        return (T)_components.GetValueOrDefault(typeof(T))! ?? throw new InvalidOperationException();
    }
}