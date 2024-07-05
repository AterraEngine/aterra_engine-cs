// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Nexities.Data.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public abstract class IOverridesAssetIdAttribute : Attribute {
    public abstract AssetId AssetId { get; }
}
