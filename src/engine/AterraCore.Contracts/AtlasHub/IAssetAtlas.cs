// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Types;

namespace AterraCore.Contracts.AtlasHub;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public IReadOnlyDictionary<AssetId, Type?> Dictionary { get; }
    
    public bool TryRegisterAsset<T>(PluginId pluginId, PartialAssetId partialAssetId, out AssetId? registeredId);
}