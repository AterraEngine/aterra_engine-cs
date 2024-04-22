// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Common;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string partialId,
    AssetInstanceType instanceType,
    CoreTags coreTags
    
) : Attribute, IAssetAttribute {
    
    public PartialAssetId PartialAssetId { get; } = new(partialId);
    public AssetInstanceType InstanceType { get; } = instanceType;
    public CoreTags CoreTags { get; } = coreTags | CoreTags.Asset;
}
