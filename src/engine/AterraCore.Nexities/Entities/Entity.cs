// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Entity<T>(T assetDto) : Asset<T>(assetDto), IComponent where T : IAssetDto{
    public HashSet<Guid> ComponentIds { get; } = [..assetDto.StartingComponentGuids];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IComponent[] GetComponents() {
        throw new NotImplementedException();
    }
}