﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault;
using Raylib_cs;

namespace AterraCore.Contracts.Nexities.Data.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISprite2D : INexitiesComponent, ITexture2DAsset {
    public Rectangle Selection { get; }
}
