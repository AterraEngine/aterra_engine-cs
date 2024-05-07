﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using AterraCore.Contracts.Nexities.Assets;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetInstance : IAssetInstance  {
    public Guid Guid { get; } = new();
    // internal AssetId? _AssetId { get; set; } // Add this for live debugging
}