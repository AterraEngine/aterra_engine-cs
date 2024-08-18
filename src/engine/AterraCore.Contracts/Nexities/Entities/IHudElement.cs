﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities.QuickHands;

namespace AterraCore.Contracts.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHudElement : INexitiesEntity,
    IHasDirectChildren,
    IHasHudComponent;
