// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Types;

namespace AterraCore.Contracts.AtlasHub;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IInstanceAtlas {
    public IReadOnlyDictionary<Guid, IAsset> Dictionary { get; }

    public bool TryCreateInstance<TDto>(AssetId assetId, TDto assetDto, [NotNullWhen(true)] out IAsset? asset) 
        where TDto : IAssetDto;
    public bool TryCreateInstance<T, TDto>(AssetId assetId, TDto assetDto, [NotNullWhen(true)] out T? asset) 
        where T : IAsset 
        where TDto : IAssetDto;

    public bool TryRemoveInstance(Guid instanceId);
}