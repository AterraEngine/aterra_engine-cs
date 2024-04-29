// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Common;
using AterraCore.Common.Nexities;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Codeee
// ---------------------------------------------------------------------------------------------------------------------
public class Asset<T>(T assetDto) : IAsset where T : IAssetDto {

    public Guid Guid { get; } = new();
    public AssetId AssetId { get; } = assetDto.AssetId;

}