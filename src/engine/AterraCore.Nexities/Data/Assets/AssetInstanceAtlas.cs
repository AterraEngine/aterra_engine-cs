// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Assets;

using AterraCore.Common.Nexities;
using AterraCore.DI;
using Contracts.Nexities.Data.Assets;
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
        return true;
    }
}