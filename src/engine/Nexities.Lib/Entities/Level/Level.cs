// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Nexities.Entities;

namespace Nexities.Lib.Entities.Level;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Level(IAssetTree childEntities) : NexitiesEntity(childEntities), ILevel {
    public IAssetTree ChildEntities => childEntities;
}