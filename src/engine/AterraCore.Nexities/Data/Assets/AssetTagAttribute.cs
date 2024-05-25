// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Assets;

using Contracts.Nexities.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AssetTagAttribute(params string[]? tags) : AbstractAssetTagAttribute {
    public override string[] Tags { get; } = tags?.ToArray() ?? [];
}