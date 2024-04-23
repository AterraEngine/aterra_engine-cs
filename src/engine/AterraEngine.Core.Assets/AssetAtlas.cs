// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Common;
using AterraEngine.Contracts.Core.Assets;
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
    private readonly AssetDictionary _ecsComponents = new();

    private readonly ConcurrentDictionary<AssetId, AssetInstanceType> _assetToTypes = new();
    private readonly Dictionary<AssetInstanceType, AssetDictionary> _funcs = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public AssetAtlas(ILogger logger) {
        _logger = logger;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetAsset(AssetId assetId, [NotNullWhen(true)] out IAsset? asset) {
        asset = null;
        return true;
    }
    public bool TryGetAsset<T>(AssetId assetId, [NotNullWhen(true)] out T? asset) where T : class {
        asset = null;
        return true;
    }

    public IReadOnlyDictionary<AssetId, IAsset> GetAllFromType(AssetInstanceType assetType) {
        return _funcs[assetType].AsReadOnly();
    }


}