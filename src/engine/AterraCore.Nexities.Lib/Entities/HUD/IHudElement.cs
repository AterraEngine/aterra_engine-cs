// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Contracts.Nexities.Data.Entities;
using AterraCore.Nexities.Lib.Components.HUD;

namespace AterraCore.Nexities.Lib.Entities.HUD;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHudElement : INexitiesEntity,
    IHasAssetTree,
    IHasHudComponent;
