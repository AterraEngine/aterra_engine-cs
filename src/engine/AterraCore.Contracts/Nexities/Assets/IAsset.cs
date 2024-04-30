﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;

namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAsset {
    public Guid Guid { get; }
    public AssetId AssetId { get; }
}