// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Nexities.Lib.Components.Sprite2D;
using Nexities.Lib.Components.Transform2D;
namespace Nexities.Lib.Entities.Actor;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Contracts.Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IActor2D : INexitiesEntity,
    IHasTransform2D,
    IHasSprite2D,
    IHasAssetTree;