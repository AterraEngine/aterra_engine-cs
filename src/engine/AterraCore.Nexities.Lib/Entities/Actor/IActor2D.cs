// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Lib.Entities.Actor;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Contracts.Nexities.Data.Entities;
using Components.Sprite2D;
using Components.Transform2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IActor2D : INexitiesEntity,
    IHasTransform2D,
    IHasSprite2D,
    IHasAssetTree;