// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraCore.Types;

namespace AterraEngine.Contracts.Core.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {
    public bool TryGetAsset(AssetId assetId, [NotNullWhen(true)] out IAsset? asset);
    public bool TryGetAsset<T>(AssetId assetId, [NotNullWhen(true)] out T? asset) where T : class;
    public IReadOnlyDictionary<AssetId, IAsset> GetAllFromType(AssetType assetType);
}