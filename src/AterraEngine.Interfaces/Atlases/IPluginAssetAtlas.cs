// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using AterraEngine.Interfaces.Actors;
using AterraEngine.Types;

namespace AterraEngine.Interfaces.Atlases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginAssetAtlas : IAssetAtlas, ITexture2DAtlas {
    public PluginId PluginId { get; set; }
    public ReadOnlyDictionary<EngineAssetId, IAsset> AssetDictionary { get; }
    public ReadOnlyDictionary<EngineAssetId, IAsset> TextureDictionary { get; }
}