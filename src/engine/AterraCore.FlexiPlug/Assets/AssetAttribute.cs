// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Types;
using Extensions;

namespace AterraCore.FlexiPlug.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(string? partialId) : Attribute {
    public PartialAssetId? PartialAssetId { get; } = partialId.IsNotNullOrEmpty() ? new PartialAssetId(partialId!) : null;
}