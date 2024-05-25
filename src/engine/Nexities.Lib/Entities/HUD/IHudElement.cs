// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Nexities.Lib.Components.HUD;
namespace Nexities.Lib.Entities.HUD;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Contracts.Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IHudElement : INexitiesEntity,
    IHasAssetTree,
    IHasHudComponent;