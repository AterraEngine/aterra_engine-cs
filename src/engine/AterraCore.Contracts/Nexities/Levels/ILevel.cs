// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Contracts.Nexities.Components.LevelData;
using AterraCore.Contracts.Nexities.Entities;

namespace AterraCore.Contracts.Nexities.Levels;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ILevel  : INexitiesEntity, IHasAssetTree, IHasLevelDataSystems;