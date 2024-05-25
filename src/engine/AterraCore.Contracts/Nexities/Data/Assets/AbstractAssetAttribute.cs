// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Contracts.Nexities.Data.Assets;

using AterraCore.Common;
using AterraCore.Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public abstract class AbstractAssetAttribute : Attribute {
    public abstract PartialAssetId PartialAssetId { get; }
    public abstract ServiceLifetimeType ServiceLifetime { get; }
    public abstract CoreTags CoreTags { get; }
    public abstract Type? InterfaceType { get; }
}