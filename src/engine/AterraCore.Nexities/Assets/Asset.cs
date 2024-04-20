// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Types;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Asset<T>(T assetDto) : IAsset where T : IAssetDto {

    public Guid Guid { get; } = new();
    public AssetId AssetId { get; } = assetDto.AssetId;

}