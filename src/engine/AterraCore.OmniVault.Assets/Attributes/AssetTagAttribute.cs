// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets.Attributes;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.Assets.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[UsedImplicitly]
public class AssetTagAttribute(params string[]? tags) : IAssetTagAttribute {
    public override string[] Tags { get; } = tags?.ToArray() ?? [];
}
