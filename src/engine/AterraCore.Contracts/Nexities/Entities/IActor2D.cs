﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities.QuickHands;

namespace AterraCore.Contracts.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActor2D : INexitiesEntity,
    IHasTransform2D,
    IHasSprite2D,
    IHasDirectChildren,
    IHasImpulse2D;
