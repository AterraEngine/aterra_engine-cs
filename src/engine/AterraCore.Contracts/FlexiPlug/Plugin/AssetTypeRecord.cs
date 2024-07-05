// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Data.Assets;

namespace AterraCore.Contracts.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record AssetTypeRecord(
    Type Type,
    AbstractAssetAttribute AssetAttribute,
    IEnumerable<AbstractOverridesAssetIdAttribute> OverwritesAssetIdAttributes,
    IEnumerable<AbstractAssetTagAttribute> AssetTagAttributes
);
