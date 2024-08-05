// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;

namespace AterraCore.OmniVault.Assets.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Asset,
    params Type[] interfaceTypes
) : IAssetAttribute {
    public override AssetId AssetId { get; } = assetId;
    public override CoreTags CoreTags { get; } = coreTags | CoreTags.Asset;
    public override Type[] InterfaceTypes { get; } = interfaceTypes;
}
