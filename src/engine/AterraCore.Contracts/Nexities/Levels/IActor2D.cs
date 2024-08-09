// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;

namespace AterraCore.Contracts.Nexities.Levels;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActor2D : INexitiesEntity,
    IHasTransform2D,
    IHasSprite2D,
    IHasAssetTree,
    IHasImpulse2D;
