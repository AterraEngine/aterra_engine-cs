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
    CoreTags coreTags,
    params Type[] interfaceTypes
) : AbstractAssetAttribute {
    public override AssetId AssetId { get; } = new(assetId);
    public override CoreTags CoreTags { get; } = coreTags | CoreTags.Asset;
    public override Type[] InterfaceTypes { get; } = interfaceTypes;
}
