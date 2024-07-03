// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.DI;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Nexities.Assets;

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
    public bool TryCreateInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : IAssetInstance {
        instance = default;
        if (!assetAtlas.TryGetRegistration(assetId, out AssetRegistration registration)) {
            logger.Warning("Asset Id {id} could not be matched to a Type", assetId);
            return false;
        }

        // Will work for Entities which have DI injected components,
        //      because all components have their interface mapped to their assetId
        // Will work for others as well
        instance = EngineServices.CreateWithServices<T>(registration.Type);
        instance.AssetId = registration.AssetId;

        // Update the generated
        instance.Guid = predefinedGuid ?? Guid.NewGuid();

        // Finally add the instance
        return _assetInstances.TryAdd(instance.Guid, instance);
    }

    public bool TryCreateInstance<T>([NotNullWhen(true)] out T? instance) where T : IAssetInstance {
        instance = default;
        if (assetAtlas.TryGetAssetId<T>(out AssetId assetId)) return TryCreateInstance(assetId, out instance);
        logger.Warning("Type {T} could not be matched to a assetId", typeof(T));
        return false;
    }
    
    public bool TryCreateInstance(Type type, [NotNullWhen(true)] out IAssetInstance? instance) {
        instance = default;
        if (assetAtlas.TryGetAssetId(type, out AssetId assetId)) return TryCreateInstance(assetId, out instance);
        logger.Warning("Type {T} could not be matched to a assetId", type);
        return false;
    }

    public bool TryGetInstance<T>(Guid instanceId, [NotNullWhen(true)] out T? instance) where T : IAssetInstance {
        instance = default;
        if (!_assetInstances.TryGetValue(instanceId, out IAssetInstance? assetInstance)) return false;
        if (assetInstance is not T convertedInstance) return false;
        instance = convertedInstance;
        return true;
    }
}
