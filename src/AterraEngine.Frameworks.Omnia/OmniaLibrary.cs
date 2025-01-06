// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaLibrary : IOmniaLibrary {
    
    // TODO update to frozen when engine is running
    public ConcurrentDictionary<OmniaId, OmniaTypeRegistration> Assets { get; } = new();
    public ConcurrentDictionary<Type, OmniaId> AssetsByType { get; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Result<TAsset> CreateAsset<TAsset>() where TAsset : IOmniaAsset {
        if (!AssetsByType.TryGetValue(typeof(TAsset), out OmniaId omniaId)) return Result<TAsset>.FromError("Asset type not found");
        if (!Assets.TryGetValue(omniaId, out OmniaTypeRegistration registration)) return Result<TAsset>.FromError("Asset registration not found");
        if (registration.TryGetAssetFromPool(omniaId, out TAsset? pooledInstance)) return pooledInstance;
        if (!registration.TryGetAssetFromFactory(out pooledInstance)) return Result<TAsset>.FromError("Failed to create asset");
        return pooledInstance;
    }
    
    public void ReturnAsset<TAsset>(TAsset instance) where TAsset : IOmniaAsset {
        if (!AssetsByType.TryGetValue(typeof(TAsset), out OmniaId omniaId)) return ;
        if (!Assets.TryGetValue(omniaId, out OmniaTypeRegistration registration)) return ;
        if (!registration.ReturnInstance(instance)) return ;
        return;
    }

    public void RegisterAsset(Type type, OmniaId omniaId, IOmniaFactory factory) {
        Assets.TryAdd(omniaId, OmniaTypeRegistration.FromFactory(factory));
        AssetsByType.TryAdd(type, omniaId);
    }

    public void ClearCaches() {
        foreach (OmniaTypeRegistration registration in Assets.Values) registration.Pool.Clear();
    }
    
    
    public Result<TAsset> FindOrCreateAsset<TAsset>(Guid someGuid) where TAsset : class {
        if (!AssetsByType.TryGetValue(typeof(TAsset), out OmniaId omniaId)) return Result<TAsset>.FromError("Asset type not found");
        if (!Assets.TryGetValue(omniaId, out OmniaTypeRegistration registration)) return Result<TAsset>.FromError("Asset registration not found");
        
        if (registration.Instances.TryGetValue(someGuid, out IOmniaAsset? instance)) return (instance as TAsset)!;
        
        // ELSE CREATE IT WITH THE SPECIFIC ID
        return Result<TAsset>.FromError("Asset not found");
    }
}
