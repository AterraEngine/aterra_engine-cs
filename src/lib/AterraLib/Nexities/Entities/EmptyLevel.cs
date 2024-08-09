// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;

namespace AterraLib.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity<INexitiesLevel>("AterraLib:Nexities/Levels/Empty")]
[UsedImplicitly]
public class EmptyLevel(IAssetTree assetTree) : NexitiesEntity(assetTree), INexitiesLevel {
    public IAssetTree AssetTree { get; } = assetTree;
}

