// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Assets;

using AterraCore.Contracts.Nexities.Data.Assets;
using Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string partialId,
    ServiceLifetimeType serviceLifetime,
    CoreTags coreTags,
    Type? interfaceType = null
) : AbstractAssetAttribute {

    public override PartialAssetId PartialAssetId { get; } = new(partialId);
    public override ServiceLifetimeType ServiceLifetime { get; } = serviceLifetime;
    public override CoreTags CoreTags { get; } = coreTags | CoreTags.Asset;
    public override Type? InterfaceType { get; }= interfaceType; // Might be defined, depending on the asset in question
}