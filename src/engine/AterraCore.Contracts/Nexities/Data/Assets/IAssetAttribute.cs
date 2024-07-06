// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Nexities.Data.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public abstract class IAssetAttribute : Attribute {
    public abstract AssetId AssetId { get; }
    public abstract CoreTags CoreTags { get; }
    public abstract Type[] InterfaceTypes { get; }
}
