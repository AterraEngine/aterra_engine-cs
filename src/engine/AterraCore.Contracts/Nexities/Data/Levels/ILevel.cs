// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Contracts.Nexities.Data.Components.LevelData;
using AterraCore.Contracts.Nexities.Data.Entities;

namespace AterraCore.Contracts.Nexities.Data.Levels;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILevel : INexitiesEntity, IHasAssetTree, IHasLevelDataSystems;
