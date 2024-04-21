﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;

namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAttribute {
    public PartialAssetId PartialAssetId { get;}
    public AssetInstanceType InstanceType { get;}
    public CoreTags CoreTags { get; }
}