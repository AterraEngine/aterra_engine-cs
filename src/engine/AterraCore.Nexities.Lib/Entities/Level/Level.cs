// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;
namespace AterraCore.Nexities.Lib.Entities.Level;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Contracts.Nexities.Data.Components.LevelData;
using AterraCore.Contracts.Nexities.Data.Levels;
using Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<ILevel>("AE000001")]
public class Level(IAssetTree childEntities, ILevelDataSystems levelDataSystems) : NexitiesEntity(childEntities), ILevel {
    public IAssetTree ChildEntities => childEntities;
    public ILevelDataSystems Systems => levelDataSystems;
}