// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Assets;

using AterraCore.Common.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.DI;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetInstanceAtlas(ILogger logger, IAssetAtlas assetAtlas) : IAssetInstanceAtlas {
    private readonly ConcurrentDictionary<Guid, IAssetInstance> _assetInstances = new();

    public int TotalCount => _assetInstances.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryCreateInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : IAssetInstance {
        instance = default;
        if (!assetAtlas.TryGetType(assetId, out Type? type)) {
            logger.Warning("Asset Id {id} could not be matched to a Type", assetId);
            return false;
        }

        instance = EngineServices.CreateWithServices<T>(type);
        instance.AssetId = assetId;
        return true;
    }

    public bool TryGetInstance<T>(Guid instanceId, [NotNullWhen(true)] out T? instance) where T : IAssetInstance {
        instance = default;
        if (!_assetInstances.TryGetValue(instanceId, out IAssetInstance? assetInstance )) return false;
        if (assetInstance is not T convertedInstance) return false;
        instance = convertedInstance;
        return true;
    }
    
}