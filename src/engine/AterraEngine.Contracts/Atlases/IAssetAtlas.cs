// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Assets;
using AterraEngine.Core.Types;

namespace AterraEngine.Contracts.Atlases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {

    public bool TryGetAsset(EngineAssetId assetId, [NotNullWhen(true)] out IEngineAsset? asset);
    public bool TryGetAsset<T>(EngineAssetId assetId, [NotNullWhen(true)] out T? asset) where T : IEngineAsset?;
    
    public bool TryRegisterAsset(IEngineAsset asset);

    public IReadOnlyDictionary<EngineAssetId, ILevel> GetAllLevels();
    public bool TryGetLevel<T>(EngineAssetId assetId, [NotNullWhen(false)] out T? asset) where T : ILevel?;
}