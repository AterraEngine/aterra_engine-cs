// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Entities;

using Assets;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;
using Common.Nexities;
using System.Collections.Concurrent;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(params IComponent[]? components) : AssetInstance, INexitiesEntity {
    protected readonly ConcurrentDictionary<AssetId, IComponent> ComponentsCache = CreateFromComponents(components ?? []);
    public IEnumerable<IComponent> Components => ComponentsCache.Values;


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static ConcurrentDictionary<AssetId, IComponent> CreateFromComponents(IEnumerable<IComponent> components) {
        return  new ConcurrentDictionary<AssetId, IComponent>(
            components
                // The reason we ! Null possibility of component.AssetId is because when an entity is created
                //      the asset instance atlas creates the instance & populates the asset id
                .ToDictionary(component => (AssetId)component.AssetId! , component => component)
                .AsEnumerable());
    }
    
}