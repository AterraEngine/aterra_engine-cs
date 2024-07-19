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
    private readonly ConcurrentDictionary<AssetId, INexitiesComponent> _components = new(
        components.Select(component => new KeyValuePair<AssetId,INexitiesComponent>(component.AssetId, component))
    );
    protected readonly ConcurrentDictionary<AssetId, AssetId> ComponentTypes = new();

    public ICollection<INexitiesComponent> Components => _components.Values;
    public ICollection<AssetId> ComponentAssetIds => _components.Keys;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Component manipulation Methods
    // -----------------------------------------------------------------------------------------------------------------
    public T GetComponent<T>(AssetId assetId) where T : INexitiesComponent {
        if (!TryGetComponent(assetId, out INexitiesComponent? nexitiesComponent)) throw new ArgumentException($"Component with assetId {assetId} not found");
        return (T)nexitiesComponent;
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
    
    public bool TryAddComponent(INexitiesComponent component) {
        return _components.TryAdd(component.AssetId, component) 
               && ComponentTypes.TryAdd(component.AssetId, component.AssetId);
    }

    public bool TryOverwriteComponent(INexitiesComponent component) =>
        TryOverwriteComponent(component, out _);
    public bool TryOverwriteComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        _components.TryGetValue(component.AssetId, out oldComponent)
        && _components.TryUpdate(component.AssetId, component, oldComponent)
        && ComponentTypes.TryGetValue(oldComponent.AssetId, out AssetId oldAssetId)
        && ComponentTypes.TryUpdate(oldComponent.AssetId, component.AssetId, oldAssetId)
        ;

    public bool TryRemoveComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        _components.TryRemove(assetId, out oldComponent)
        && ComponentTypes.TryRemove(oldComponent.AssetId, out _);
    public bool TryRemoveComponent(AssetId assetId) => 
        _components.TryRemove(assetId, out INexitiesComponent? oldComponent)
        && ComponentTypes.TryRemove(oldComponent.AssetId, out _);
    

}



















