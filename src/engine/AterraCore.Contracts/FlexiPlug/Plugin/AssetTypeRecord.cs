// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Assets.Attributes;

namespace AterraCore.Contracts.FlexiPlug.Plugin;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record AssetTypeRecord(
    Type Type,
    IAssetAttribute AssetAttribute,
    IEnumerable<IOverridesAssetIdAttribute> OverwritesAssetIdAttributes,
    IEnumerable<IAssetTagAttribute> AssetTagAttributes
);
