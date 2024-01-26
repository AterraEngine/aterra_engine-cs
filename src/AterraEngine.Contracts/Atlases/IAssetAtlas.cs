// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Actors;
using AterraEngine.Types;
namespace AterraEngine.Contracts.Atlases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {

    public bool TryGetAsset(EngineAssetId assetId, [MaybeNullWhen(false)] out IAsset asset);
    public bool TryGetAsset<T>(EngineAssetId assetId, [MaybeNullWhen(false)] out T asset) where T : IAsset?;
    
    public bool TryRegisterAsset(IAsset asset);

    public IReadOnlyDictionary<EngineAssetId, ILevel> GetAllLevels();
    public bool TryGetLevel<T>(EngineAssetId assetId, [MaybeNullWhen(false)] out T asset) where T : ILevel?;
}