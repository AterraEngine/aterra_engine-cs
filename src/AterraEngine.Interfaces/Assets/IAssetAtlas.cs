// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using AterraEngine_lib.structs;

namespace AterraEngine.Interfaces.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {
    public ReadOnlyDictionary<EngineAssetId, IAsset> Assets { get; }
    public bool TryGetAsset(EngineAssetId value, out IAsset? asset);
    public bool TryGetAsset(string value, out IAsset? asset);

    public bool TryAddAsset(IAsset asset);
    public bool TryParseAssetIdFromString(string value, out EngineAssetId? engineAssetId);

}