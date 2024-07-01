// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string assetId,
    ServiceLifetimeType serviceLifetime,
    CoreTags coreTags,
    Type? interfaceType = null
    ) : AbstractAssetAttribute {
    public override AssetId AssetId { get; } = new(assetId);
    public override ServiceLifetimeType ServiceLifetime { get; } = serviceLifetime;
    public override CoreTags CoreTags { get; } = coreTags | CoreTags.Asset;
    public override Type? InterfaceType { get; } = interfaceType;// Might be defined, depending on the asset in question
}
