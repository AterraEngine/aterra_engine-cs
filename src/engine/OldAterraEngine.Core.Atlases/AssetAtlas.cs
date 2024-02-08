// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Atlases;
using OldAterraEngine.Core.Types;

namespace OldAterraEngine.Core.Atlases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetAtlas: IAssetAtlas {
    private readonly ConcurrentDictionary<EngineAssetId, IEngineAsset?> _dictionary = new();

    public bool TryGetAsset(EngineAssetId assetId, [NotNullWhen(true)] out IEngineAsset? asset) => TryGetAsset<IEngineAsset>(assetId, out asset);
    public bool TryGetAsset<T>(EngineAssetId assetId, [NotNullWhen(true)] out T? asset) where T : IEngineAsset {
        if (_dictionary.TryGetValue(assetId, out IEngineAsset? tempAsset) && tempAsset is T value) {
            asset = value;
            return true;
        }

        asset = default;
        return false;
    }

    public bool TryRegisterAsset(IEngineAsset asset) {
        return _dictionary.TryAdd(asset.Id, asset);
    }

    public IReadOnlyDictionary<EngineAssetId, ILevel> GetAllLevels() {
        KeyValuePair<EngineAssetId, IEngineAsset>[]? snapshot = _dictionary.ToArray();
        return snapshot
            .Where(pair => pair.Value is ILevel)
            .ToImmutableDictionary(pair => pair.Key, pair => (ILevel)pair.Value);
    }
    
    public bool TryGetLevel<T>(EngineAssetId assetId, [NotNullWhen(true)] out T? asset) where T : ILevel {
        if (GetAllLevels().TryGetValue(assetId, out ILevel? tempAsset) && tempAsset is T value) {
            asset = value;
            return true;
        }

        asset = default;
        return false;
    }
}