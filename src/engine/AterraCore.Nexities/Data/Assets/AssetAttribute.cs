// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Assets;

using AterraCore.Common;
using AterraCore.Common.Nexities;
using Contracts.Nexities.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string partialId,
    AssetInstanceType instanceType,
    CoreTags coreTags
) : AbstractAssetAttribute {

    public override PartialAssetId PartialAssetId { get; } = new(partialId);
    public override AssetInstanceType InstanceType { get; } = instanceType;
    public override CoreTags CoreTags { get; } = coreTags | CoreTags.Asset;
}