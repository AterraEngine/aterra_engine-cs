// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.AtlasHub;
using AterraCore.Contracts.Nexities;
using AterraCore.Types;

namespace AterraCore.AtlasHub.AssetAtlas;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetAtlas : IAssetAtlas {
    private readonly ConcurrentDictionary<AssetId, AssetRecord> _dictionary = new();
    public IReadOnlyDictionary<AssetId, AssetRecord> Dictionary => _dictionary.AsReadOnly();

    private readonly ConcurrentBag<Type> _types = []; // Normally this shouldn't really be needed, but you never know.
    
    // ---------------------------------------------------------------------------------------------------------------------
    // Methods
    // ---------------------------------------------------------------------------------------------------------------------
    public AssetRecord this[AssetId id] => _dictionary.TryGetValue(id, out AssetRecord value) ? value : null;
    
    public bool TryRegisterAsset<T>(PluginId pluginId, PartialAssetId partialAssetId, [NotNullWhen(true)] out AssetId? registeredId) {
        registeredId = null;
        Type type = typeof(T);

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

    public bool TryGetAssetRecord(AssetId assetId, [NotNullWhen(true)] out Type? type) {
        return _dictionary.TryGetValue(assetId, out type);
    }
    
    public bool TryGetAssetType(string assetId, [NotNullWhen(true)] out Type? type) {
        return TryGetAssetRecord(new AssetId(assetId), out type);
    }
    
}