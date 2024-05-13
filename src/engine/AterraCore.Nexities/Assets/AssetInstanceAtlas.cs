// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.DI;
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
    public bool TryCreateInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : IAssetInstance {
        instance = default;
        if (!assetAtlas.TryGetType(assetId, out Type? type)) {
            logger.Warning("Asset Id {id} could not be matched to a Type", assetId );
            return false;
        }

        instance = EngineServices.CreateWithServices<T>(type);
        return true;
    }
}