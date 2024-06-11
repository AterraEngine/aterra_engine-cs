// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Nexities.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public abstract class AbstractAssetAttribute : Attribute {
    public abstract PartialAssetId PartialAssetId { get; }
    public abstract ServiceLifetimeType ServiceLifetime { get; }
    public abstract CoreTags CoreTags { get; }
    public abstract Type? InterfaceType { get; }
}
