// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.Assets;

using AterraCore.Contracts.Nexities.Data.Assets;
using Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetInstance : IAssetInstance {
    public Guid Guid { get; set; }// Internal set because we need to not have randomly generated when loading from file
    public AssetId? AssetId { get; set; }
}