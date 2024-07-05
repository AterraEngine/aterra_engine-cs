// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
namespace AterraCore.Nexities.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class OverridesAssetIdAttribute(string assetId) : AbstractOverridesAssetIdAttribute {
    public override AssetId AssetId { get; } = new(assetId);
}
