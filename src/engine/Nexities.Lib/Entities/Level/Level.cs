// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Contracts.Nexities.Components.LevelData;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;
namespace Nexities.Lib.Entities.Level;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<ILevel>("AE000001")]
public class Level(IAssetTree childEntities, ILevelDataSystems levelDataSystems) : NexitiesEntity(childEntities), ILevel {
    public IAssetTree ChildEntities => childEntities;
    public ILevelDataSystems Systems => levelDataSystems;
}