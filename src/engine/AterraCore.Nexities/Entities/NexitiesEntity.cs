// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;
using AterraCore.OmniVault.Assets;
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
    private readonly ConcurrentDictionary<Type, AssetId> _componentTypes = new(
        components.Select(component => new KeyValuePair<Type,AssetId>(component.GetType(), component.AssetId))
    );

    public ICollection<INexitiesComponent> Components => _components.Values;
    public ICollection<AssetId> ComponentAssetIds => _components.Keys;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Component manipulation Methods
    // -----------------------------------------------------------------------------------------------------------------
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
            AssetId assetId = _componentTypes[typeof(T)]; 
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
    
    public bool TryAddComponent(INexitiesComponent component) {
        return _components.TryAdd(component.AssetId, component) 
               && _componentTypes.TryAdd(component.GetType(), component.AssetId);
    }

    public bool TryOverwriteComponent(INexitiesComponent component) =>
        TryOverwriteComponent(component, out _);
    public bool TryOverwriteComponent(INexitiesComponent component, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        _components.TryGetValue(component.AssetId, out oldComponent)
        && _components.TryUpdate(component.AssetId, component, oldComponent)
        && _componentTypes.TryGetValue(oldComponent.GetType(), out AssetId oldAssetId)
        && _componentTypes.TryUpdate(oldComponent.GetType(), component.AssetId, oldAssetId)
        ;

    public bool TryRemoveComponent(AssetId assetId, [NotNullWhen(true)] out INexitiesComponent? oldComponent) =>
        _components.TryRemove(assetId, out oldComponent)
        && _componentTypes.TryRemove(oldComponent.GetType(), out _);
    public bool TryRemoveComponent(AssetId assetId) => 
        _components.TryRemove(assetId, out INexitiesComponent? oldComponent)
        && _componentTypes.TryRemove(oldComponent.GetType(), out _);
    

}



















