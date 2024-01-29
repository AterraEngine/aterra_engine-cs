// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.OldContracts.Assets;
using AterraEngine.Types;
namespace AterraEngine.OldContracts.Atlases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {

    public bool TryGetAsset(EngineAssetId assetId, [MaybeNullWhen(false)] out IEngineAsset asset);
    public bool TryGetAsset<T>(EngineAssetId assetId, [MaybeNullWhen(false)] out T asset) where T : IEngineAsset?;
    
    public bool TryRegisterAsset(IEngineAsset? asset);

    public IReadOnlyDictionary<EngineAssetId, ILevel> GetAllLevels();
    public bool TryGetLevel<T>(EngineAssetId assetId, [MaybeNullWhen(false)] out T asset) where T : ILevel?;
}