// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Core.ECSFramework;
using AterraEngine.Core.ServicesFramework;
using AterraEngine.Core.Types;
namespace AterraEngine.Core.ECSFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Entity(AssetId id) : IEntity {
    public AssetId Id { get; } = id;
    protected readonly ConcurrentDictionary<Type, IComponent> _components = new();
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

    public bool TryAddComponent<T>() where T : class, IComponent {
        return TryAddComponent(EngineServices.CreateWithServices<T>());
    }
    public bool TryAddComponent<T>(T component) where T : class, IComponent{
        return _components.TryAdd(typeof(T), component);
    }

    public bool TryRemoveComponent<T>( [NotNullWhen(true)] out IComponent? component) where T : IComponent {
        return _components.Remove(typeof(T), out component);
    }

    protected T GetComponent<T>() where T : IComponent {
        return (T)_components.GetValueOrDefault(typeof(T))! ?? throw new InvalidOperationException();
    }
}