// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Contracts.Nexities.Data.Components.LevelData;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Lib.Entities.Level;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<ILevel>("Nexities:Entities/Level")]
public class Level(IAssetTree childEntities, ILevelDataSystems levelDataSystems) : NexitiesEntity(childEntities), ILevel {
    public IAssetTree ChildEntities => childEntities;
    public ILevelDataSystems Systems => levelDataSystems;
}
