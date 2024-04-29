// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Common.Nexities;

namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public abstract class AbstractAssetAttribute  : Attribute{
    public abstract PartialAssetId PartialAssetId { get; }
    public abstract AssetInstanceType InstanceType { get; }
    public abstract CoreTags CoreTags { get; }
}