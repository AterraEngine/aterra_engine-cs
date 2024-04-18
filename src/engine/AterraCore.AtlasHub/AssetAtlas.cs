// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using AterraCore.Contracts.AtlasHub;
using AterraCore.Types;

namespace AterraCore.AtlasHub;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetAtlas : IAssetAtlas {
    private readonly ConcurrentDictionary<AssetId, Type?> _dictionary = new();
    public IReadOnlyDictionary<AssetId, Type?> Dictionary => _dictionary.AsReadOnly();

    private HashSet<Type?> _types = [];
    
    // ---------------------------------------------------------------------------------------------------------------------
    // Methods
    // ---------------------------------------------------------------------------------------------------------------------
    public bool TryRegisterAsset<T>(PluginId pluginId, PartialAssetId partialAssetId, out AssetId? registeredId) {
        registeredId = null;
        Type? type = typeof(T);

        if (_types.Contains(type)) {
            AssetId knownId = _dictionary.FirstOrDefault(v => v.Value == type).Key;
            throw new ArgumentException($"Type {type.Name} is already registered in the asset atlas with the AssetId of {knownId}.");
        }

        registeredId = new AssetId(pluginId, partialAssetId);
        if (!_dictionary.TryAdd((AssetId)registeredId, type)) {
            return false; 
        }

        _types.Add(type);
        return true;
    }
}