// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Entities.Pools;
using AterraCore.DI;
using AterraCore.OmniVault.Assets;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(params INexitiesComponent[] components) : AssetInstance, INexitiesEntity, IDisposable {
    #region Pooled Dictionaries
    private static INexitiesEntityPools? _pools;
    private static INexitiesEntityPools Pools => _pools ??= EngineServices.GetService<INexitiesEntityPools>();

    private readonly ConcurrentDictionary<AssetId, INexitiesComponent> _components = CreateDictionaryComponents(components);
    private readonly ConcurrentDictionary<Type, AssetId> _componentTypes = CreateDictionaryComponentTypes(components);

    private static ConcurrentDictionary<AssetId, INexitiesComponent> CreateDictionaryComponents(INexitiesComponent[] components) {
        ConcurrentDictionary<AssetId, INexitiesComponent> dict = Pools.ComponentPool.Get();
        foreach (INexitiesComponent component in components) {
            dict.TryAdd(component.AssetId, component);
        }
        return dict;
    }
    private static ConcurrentDictionary<Type, AssetId> CreateDictionaryComponentTypes(INexitiesComponent[] components) {
        ConcurrentDictionary<Type, AssetId> dict = Pools.ComponentTypePool.Get();
        foreach (INexitiesComponent component in components) {
            dict.TryAdd(component.GetType(), component.AssetId);
        }
        return dict;
    }

    public IReadOnlyCollection<INexitiesComponent> Components => _components.Values.ToArray().AsReadOnly();
    public IReadOnlyCollection<AssetId> ComponentAssetIds => _components.Keys.ToArray().AsReadOnly();
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Abstract Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected abstract void ComponentOverwritten();

    // -----------------------------------------------------------------------------------------------------------------
    // Component manipulation Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Get & TryGet Components
    public T GetComponent<T>(AssetId assetId) where T : INexitiesComponent {
        try {
            return (T)_components[assetId];
        }
        catch (Exception e) {
            throw new ArgumentException($"Component with assetId {assetId} not found", e);
        }
    }

    public T GetComponent<T>() where T : INexitiesComponent {
        try {
            AssetId assetId = _componentTypes.FirstOrDefault(kvp => typeof(T).IsAssignableFrom(kvp.Key)).Value;
            return (T)_components[assetId];
        }
        catch (Exception e) {
            throw new ArgumentException($"Component with type {typeof(T)} not found", e);
        }
    }

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T : INexitiesComponent {
        if (!(_componentTypes.TryGetValue(typeof(T), out AssetId assetId) && _components.TryGetValue(assetId, out INexitiesComponent? nexitiesComponent))) {
            component = default;
            return false;
        }
        component = (T)nexitiesComponent;
        return true;
    }

    public bool TryGetComponent<T>(AssetId assetId, [NotNullWhen(true)] out T? component) where T : INexitiesComponent {
        component = default;
        if (!TryGetComponent(assetId, out INexitiesComponent? nexitiesComponent)) return false;
        if (nexitiesComponent is not T newComponent) return false;
        component = newComponent;
        return true;
    }

    public bool TryGetComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? component) =>
        _components.TryGetValue(assetId, out component);
    #endregion
    #region Try Add
    public bool TryAddComponent(INexitiesComponent component) =>
        _components.TryAdd(component.AssetId, component)
        && _componentTypes.TryAdd(component.GetType(), component.AssetId);
    #endregion
    #region Try Overwrite
    public bool TryOverwriteComponent(INexitiesComponent component) => TryOverwriteComponent(component, out _);
    public bool TryOverwriteComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent) {
        oldComponent = null;
        if (!_components.TryGetValue(component.AssetId, out oldComponent)
            || !_componentTypes.TryGetValue(oldComponent.GetType(), out AssetId oldAssetId))
            return false;
        if (!_components.TryUpdate(component.AssetId, component, oldComponent)
            || !_componentTypes.TryUpdate(oldComponent.GetType(), component.AssetId, oldAssetId)) return false;

        ComponentOverwritten();
        return true;
    }
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Cleanup
    // -----------------------------------------------------------------------------------------------------------------
    #region Dispose
    public void Dispose() {
        // Return the dictionaries to the pool
        Pools.ComponentPool.Return(_components);
        Pools.ComponentTypePool.Return(_componentTypes);

        GC.SuppressFinalize(this);
    }
    #endregion
}
