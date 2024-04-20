// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Types;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string partialId,
    AssetInstanceType instanceType,
    CoreTags coreTags
) : Attribute {
    
    public PartialAssetId? PartialAssetId { get; private set; } = new(partialId);
    public AssetInstanceType InstanceType { get; private set; } = instanceType;
    public CoreTags CoreTags { get; private set; } = coreTags | CoreTags.Asset;
}
