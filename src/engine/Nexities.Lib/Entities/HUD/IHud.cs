﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Contracts.Nexities.Entities;
namespace Nexities.Lib.Entities.HUD;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHud : INexitiesEntity,
    IHasAssetTree;