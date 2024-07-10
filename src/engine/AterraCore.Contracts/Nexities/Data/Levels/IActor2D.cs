// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;

namespace AterraCore.Contracts.Nexities.Data.Levels;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActor2D : INexitiesEntity,
    IHasTransform2D,
    IHasSprite2D,
    IHasAssetTree;
