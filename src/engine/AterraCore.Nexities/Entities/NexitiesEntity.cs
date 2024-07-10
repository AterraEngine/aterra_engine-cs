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

    public ICollection<INexitiesComponent> Components => _components.Values;
    public ICollection<AssetId> ComponentAssetIds => _components.Keys;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Component manipulation Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? component) =>
        _components.TryGetValue(assetId, out component);
    
    public bool TryAddComponent(INexitiesComponent component) =>
        _components.TryAdd(component.AssetId, component);
    
    public bool TryOverwriteComponent(INexitiesComponent component) =>
        TryOverwriteComponent(component, out _);
    public bool TryOverwriteComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        _components.TryGetValue(component.AssetId, out oldComponent)
        && _components.TryUpdate(component.AssetId, component, oldComponent)
        ;

    public bool TryRemoveComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        _components.TryRemove(assetId, out oldComponent);
    public bool TryRemoveComponent(AssetId assetId) => 
        _components.TryRemove(assetId, out _);
    

}



















