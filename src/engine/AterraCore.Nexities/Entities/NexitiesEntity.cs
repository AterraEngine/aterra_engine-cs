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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[DebuggerDisplay("ComponentsById: {_componentsById}")]
[DebuggerDisplay("ComponentsByType: {_componentsByType}")]
public abstract class NexitiesEntity(params INexitiesComponent[] components) : AssetInstance, INexitiesEntity, IDisposable {
    #region Pooled Dictionaries
    private static INexitiesEntityPools? _pools;
    private static INexitiesEntityPools Pools => _pools ??= EngineServices.GetService<INexitiesEntityPools>();

    private readonly ConcurrentDictionary<AssetId, INexitiesComponent> _componentsById = CreateComponentsById(components);
    private readonly ConcurrentDictionary<Type, AssetId> _componentsByType = CreateComponentsByType(components);
    private readonly ConcurrentDictionary<Ulid, AssetId> _componentsByInstanceId = CreateComponentsByInstanceId(components);

    private static ConcurrentDictionary<AssetId, INexitiesComponent> CreateComponentsById(INexitiesComponent[] components) {
        ConcurrentDictionary<AssetId, INexitiesComponent> dict = Pools.ComponentsByIdPool.Get();
        foreach (INexitiesComponent component in components) {
            dict.TryAdd(component.AssetId, component);
        }
        return dict;
    }
    private static ConcurrentDictionary<Type, AssetId> CreateComponentsByType(INexitiesComponent[] components) {
        ConcurrentDictionary<Type, AssetId> dict = Pools.ComponentsByTypePool.Get();
        foreach (INexitiesComponent component in components) {
            dict.TryAdd(component.GetType(), component.AssetId);
        }
        return dict;
    }
    private static ConcurrentDictionary<Ulid, AssetId> CreateComponentsByInstanceId(INexitiesComponent[] components) {
        ConcurrentDictionary<Ulid, AssetId> dict = Pools.ComponentsByInstanceIdPool.Get();
        foreach (INexitiesComponent component in components) {
            dict.TryAdd(component.InstanceId, component.AssetId);
        }
        return dict;
    }

    private INexitiesComponent[]? _components;
    public IReadOnlyCollection<INexitiesComponent> Components => (_components ??= _componentsById.Values.ToArray()).AsReadOnly();

    private AssetId[]? _componentIds;
    public IReadOnlyCollection<AssetId> ComponentAssetIds => (_componentIds ??= _componentsById.Keys.ToArray()).AsReadOnly();

    private Ulid[]? _componentUlids;
    public IReadOnlyCollection<Ulid> ComponentInstanceIds => (_componentUlids ??= _componentsByInstanceId.Keys.ToArray()).AsReadOnly();
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Abstract Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected virtual void ClearCaches() {
        _components = null;
        _componentIds = null;
        _componentUlids = null;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Component manipulation Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Get & TryGet Components
    public T GetComponent<T>(AssetId assetId) where T : INexitiesComponent {
        try {
            return (T)_componentsById[assetId];
        }
        catch (Exception e) {
            throw new ArgumentException($"Component with assetId {assetId} not found", e);
        }
    }

    public T GetComponent<T>() where T : INexitiesComponent {
        try {
            AssetId assetId = _componentsByType.FirstOrDefault(kvp => typeof(T).IsAssignableFrom(kvp.Key)).Value;
            return (T)_componentsById[assetId];
        }
        catch (Exception e) {
            throw new ArgumentException($"Component with type {typeof(T)} not found", e);
        }
    }

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T : INexitiesComponent {
        component = default;
        if (!_componentsByType.TryGetValue(typeof(T), out AssetId assetId)) return false;
        if (!_componentsById.TryGetValue(assetId, out INexitiesComponent? nexitiesComponent)) return false;

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
        _componentsById.TryGetValue(assetId, out component);
    #endregion
    #region Try Add
    public bool TryAddComponent(INexitiesComponent component) {
        if (!_componentsById.TryAdd(component.AssetId, component)) return false;
        if (!_componentsByType.TryAdd(component.GetType(), component.AssetId)) {
            _componentsById.TryRemove(component.AssetId, out _);
            return false;
        }

        ClearCaches();
        return true;
    }
    #endregion
    #region Try Overwrite
    public bool TryOverwriteComponent(INexitiesComponent component) => TryOverwriteComponent(component, out _);
    public bool TryOverwriteComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent) {
        oldComponent = null;
        if (!_componentsById.TryGetValue(component.AssetId, out oldComponent)) return false;
        if (!_componentsById.TryUpdate(component.AssetId, component, oldComponent)) return false;
        if (!_componentsByType.TryUpdate(oldComponent.GetType(), component.AssetId, oldComponent.AssetId)) return false;
        if (!_componentsByInstanceId.TryRemove(oldComponent.InstanceId, out _)) return false;
        if (!_componentsByInstanceId.TryAdd(oldComponent.InstanceId, component.AssetId)) return false;

        ClearCaches();
        return true;
    }
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Cleanup
    // -----------------------------------------------------------------------------------------------------------------
    #region Dispose
    public void Dispose() {
        // Return the dictionaries to the pool
        Pools.ComponentsByIdPool.Return(_componentsById);
        Pools.ComponentsByTypePool.Return(_componentsByType);
        Pools.ComponentsByInstanceIdPool.Return(_componentsByInstanceId);

        GC.SuppressFinalize(this);
    }
    #endregion
}
