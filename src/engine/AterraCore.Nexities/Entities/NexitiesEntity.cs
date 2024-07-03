// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;
using AterraCore.Nexities.Assets;
using Serilog.Core;
using System.Collections.Concurrent;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(params IComponent[]? components) : AssetInstance, INexitiesEntity {
    public ConcurrentDictionary<AssetId, IComponent> Components { get; set; } = CreateFromComponents(components ?? []);
    public IEnumerable<IComponent> ComponentsArray => Components.Values;


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static ConcurrentDictionary<AssetId, IComponent> CreateFromComponents(IEnumerable<IComponent> components) {
        return new ConcurrentDictionary<AssetId, IComponent>(
        components
            // The reason we ! Null possibility of component.AssetId is because when an entity is created
            //      the asset instance atlas creates the instance & populates the asset id
            .ToDictionary(keySelector: component => {
                Console.WriteLine(component.AssetId);
                Console.WriteLine(component.Guid);
                return component.AssetId;
            }, elementSelector: component => component)
            .AsEnumerable()
        );
    }
}
