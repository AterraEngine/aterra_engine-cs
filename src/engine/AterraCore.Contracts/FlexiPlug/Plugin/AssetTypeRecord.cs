// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;

namespace AterraCore.Contracts.FlexiPlug.Plugin;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record AssetTypeRecord(
    Type Type,
    AssetAttribute AssetAttribute,
    IEnumerable<OverridesAssetIdAttribute> OverwritesAssetIdAttributes,
    IEnumerable<AssetTagAttribute> AssetTagAttributes
);
