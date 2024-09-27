// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Common.Attributes.Nexities;

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
