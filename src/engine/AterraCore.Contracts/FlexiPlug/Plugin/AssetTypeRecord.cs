// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Attributes;

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
