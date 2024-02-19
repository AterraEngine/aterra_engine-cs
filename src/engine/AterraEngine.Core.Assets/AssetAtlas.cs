// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Core.Assets;
using AterraEngine.Core.Types;
using Serilog;

namespace AterraEngine.Core.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
using AssetDictionary = ConcurrentDictionary<AssetId, IAsset>; // Type alias

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetAtlas : IAssetAtlas {
    private ILogger _logger;
    
    private readonly AssetDictionary _undefined = new();
    private readonly AssetDictionary _actors = new();
    private readonly AssetDictionary _textures = new();
    private readonly AssetDictionary _models = new(); // empty for a long time, needed for 3d only?
    private readonly AssetDictionary _audios = new();
    private readonly AssetDictionary _levels = new();

    private readonly ConcurrentDictionary<AssetId, AssetType> _assetToTypes = new();
    private readonly Dictionary<AssetType, AssetDictionary> _funcs = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public AssetAtlas(ILogger logger) {
        _logger = logger;
        
        _funcs[AssetType.Undefined] =       _undefined;
        _funcs[AssetType.StaticActor] =     _actors;
        _funcs[AssetType.DynamicActor] =    _actors;
        _funcs[AssetType.Texture] =         _textures;
        _funcs[AssetType.Model] =           _models;
        _funcs[AssetType.Audio] =           _audios;
        _funcs[AssetType.Level] =           _levels;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetAsset(AssetId assetId, [NotNullWhen(true)] out IAsset? asset) {
        AssetType assetType = _assetToTypes.GetValueOrDefault(assetId, AssetType.Undefined);

        if (assetType is AssetType.Undefined) {
            _logger.Warning("AssetId {assetId} could not be mapped to a predefined sub-atlas", assetId);
            // should not return here, as this is simply some information?
        }
        
        // suppress the warning here, because all types are defined
        asset = _funcs[assetType]!.GetValueOrDefault(assetId, null);
        
        if (asset is null) {
            _logger.Warning("Asset could not be found with the id of {assetId}", assetId);
            return false;
        }
        
        _logger.Debug("Asset with id {assetId} found as {asset} ", assetId, asset);
        return true;
    }

    public bool TryGetAsset<T>(AssetId assetId, [NotNullWhen(true)] out T? asset) where T : class {
        asset = default;
        if (!TryGetAsset(assetId, out IAsset? asset1))  return false; // logging handled by larger TryGetAsset
        asset = asset1 as T;

        if (asset is null) {
            _logger.Warning("Asset {assetId} could not be case to {T}", assetId, typeof(T));
            return false;
        }
        
        _logger.Debug("Asset cast as {Name}", typeof(T));
        return true;
    }



}