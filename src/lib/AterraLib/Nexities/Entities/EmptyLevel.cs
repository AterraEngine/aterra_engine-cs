// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity<INexitiesLevel>("AterraLib:Nexities/Levels/Empty")]
[UsedImplicitly]
public class EmptyLevel(IAssetTree assetTree) : NexitiesEntity(assetTree), INexitiesLevel {
    public IAssetTree AssetTree { get; } = assetTree;
}

