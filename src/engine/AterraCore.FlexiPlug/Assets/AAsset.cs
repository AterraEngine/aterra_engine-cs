// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.FlexiPlug;
using AterraCore.Types;

namespace AterraCore.FlexiPlug.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AAsset(AssetDto assetDto) : IAsset {
    public AssetId Id { get; } = new(assetDto.PluginId, assetDto.PartialAssetId);
}

public abstract class AAsset<T>(T assetDto) : AAsset(assetDto) where T : AssetDto;