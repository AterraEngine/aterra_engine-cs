// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Types;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string? partialId,
    AssetType type,
    AssetInstanceType instanceType
    
    ) : Attribute {
    
    public PartialAssetId? PartialAssetId { get; } = partialId is not null ? new(partialId) : null; // This is nullable to allow for custom assets on runtime, or simply things you don't care about as a developer and simply exist as test values or something
    public AssetType Type { get; } = type;
    public AssetInstanceType InstanceType { get; } = instanceType;
}
