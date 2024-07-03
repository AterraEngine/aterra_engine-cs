// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(IEnumerable<IComponent>? nestedComponents = null, params IComponent[] components) : AssetInstance, INexitiesEntity {
    public IEnumerable<IComponent> ComponentsArray => components.Concat(nestedComponents ?? []);
}
