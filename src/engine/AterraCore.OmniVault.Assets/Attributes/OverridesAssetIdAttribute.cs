// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets.Attributes;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.Assets.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[UsedImplicitly]
public class OverridesAssetIdAttribute(string assetId) : IOverridesAssetIdAttribute {
    public override AssetId AssetId { get; } = new(assetId);
}
