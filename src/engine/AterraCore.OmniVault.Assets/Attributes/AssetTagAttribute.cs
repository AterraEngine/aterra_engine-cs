// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets.Attributes;

namespace AterraCore.OmniVault.Assets.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AssetTagAttribute(params string[]? tags) : IAssetTagAttribute {
    public override string[] Tags { get; } = tags?.ToArray() ?? [];
}
