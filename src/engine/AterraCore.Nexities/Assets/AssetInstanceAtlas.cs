// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.Nexities.Assets;
using JetBrains.Annotations;
using Serilog;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetInstanceAtlas(ILogger logger, IAssetAtlas assetAtlas) : IAssetInstanceAtlas {
    private ConcurrentDictionary<Guid, IAssetInstance> _assetInstances = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryCreateInstance(AssetId assetId, out IAssetInstance? instance) {
        instance = null;
        if (!assetAtlas.TryGetType(assetId, out Type? type)) {
            logger.Warning("Asset Id {id} could not be matched to a Type", assetId );
            return false;
        }
        

        return true;
    }
}