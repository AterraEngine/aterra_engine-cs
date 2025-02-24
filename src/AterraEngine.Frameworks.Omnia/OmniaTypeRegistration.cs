// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct OmniaTypeRegistration(
    ConcurrentDictionary<Guid, IOmniaAsset> Instances,
    IOmniaFactory Factory,
    ConcurrentQueue<IOmniaAsset> Pool
) {
    public static OmniaTypeRegistration FromFactory(IOmniaFactory factory) => new(
        new ConcurrentDictionary<Guid, IOmniaAsset>(),
        factory,
        new ConcurrentQueue<IOmniaAsset>()
    );

    public bool TryGetAssetFromPool<TAsset>(OmniaId assetId, [NotNullWhen(true)] out TAsset? instance) where TAsset : class, IOmniaAsset {
        instance = default;
        if (!Pool.TryPeek(out IOmniaAsset? assetInstance) || assetInstance is not {} asset || asset.OmniaId != assetId) return false;

        Pool.TryDequeue(out assetInstance);
        if (assetInstance is not TAsset castedInstance) return false;
        instance = castedInstance;
        return RegisterInstance(instance);
    }

    public bool TryGetAssetFromFactory<TAsset>([NotNullWhen(true)] out TAsset? instance) where TAsset : class, IOmniaAsset {
        instance = default;
        Result<TAsset> factoryResult = Factory.CreateAsset<TAsset>();
        if (factoryResult.IsError) return false;
        instance = factoryResult.AsT;
        return RegisterInstance(instance);
    }

    private bool RegisterInstance(IOmniaAsset instance) => Instances.TryAdd(instance.InstanceId, instance);

    public bool ReturnInstance(IOmniaAsset instance) {
        if (!Instances.TryRemove(instance.InstanceId, out IOmniaAsset? removedInstance)) return false;
        removedInstance.Cleanup();
        Pool.Enqueue(removedInstance);
        return true;
    }
}
