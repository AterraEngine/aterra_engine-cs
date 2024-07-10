// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;
using AterraCore.Nexities.Assets;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(params INexitiesComponent[] components) : AssetInstance, INexitiesEntity {
    private readonly ConcurrentDictionary<AssetId, INexitiesComponent> _componentsCache = new(
        components.Select(component => new KeyValuePair<AssetId,INexitiesComponent>(component.AssetId, component))
    );

    private INexitiesComponent[]? _components;
    public INexitiesComponent[] Components => _components ??= _componentsCache.Values.ToArray();

    private AssetId[]? _assetIds;
    public AssetId[] ComponentAssetIds => _assetIds ??= _componentsCache.Keys.ToArray();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool InvalidateIfTrue(bool input) {
        if (!input) return input;
        _components = null;
        _assetIds = null;
        return input;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Component manipulation Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? component) =>
        InvalidateIfTrue(_componentsCache.TryGetValue(assetId, out component));
    
    public bool TryAddComponent(INexitiesComponent component) =>
        InvalidateIfTrue(_componentsCache.TryAdd(component.AssetId, component));
    
    public bool TryUpdateComponent(INexitiesComponent component) =>
        InvalidateIfTrue(TryUpdateComponent(component, out _));
    public bool TryUpdateComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        InvalidateIfTrue(
            _componentsCache.TryGetValue(component.AssetId, out oldComponent)
            && _componentsCache.TryUpdate(component.AssetId, component, oldComponent)
        );

    public bool TryRemoveComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        InvalidateIfTrue(_componentsCache.TryRemove(assetId, out oldComponent));
    public bool TryRemoveComponent(AssetId assetId) => 
        InvalidateIfTrue(_componentsCache.TryRemove(assetId, out _));
    

}



















