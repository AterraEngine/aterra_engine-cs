﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Assets;
using Raylib_cs;

namespace AterraCore.Contracts.OmniVault;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITexture2DAsset : IAssetInstance {
    public Texture2D? Texture2D { get; set; }
}