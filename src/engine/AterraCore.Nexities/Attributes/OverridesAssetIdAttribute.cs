// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Attributes;

namespace AterraCore.Nexities.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class OverridesAssetIdAttribute(string assetId) : IOverridesAssetIdAttribute {
    public override AssetId AssetId { get; } = new(assetId);
}
