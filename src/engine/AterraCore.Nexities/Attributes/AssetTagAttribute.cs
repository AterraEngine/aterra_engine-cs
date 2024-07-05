// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Data.Assets;

namespace AterraCore.Nexities.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AssetTagAttribute(params string[]? tags) : IAssetTagAttribute {
    public override string[] Tags { get; } = tags?.ToArray() ?? [];
}
