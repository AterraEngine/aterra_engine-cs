// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record OmniaRegistration<T>(OmniaId OmniaId) : IOmniaRegistration where T : class, IOmniaAsset{
    public Type AssetType { get; } = typeof(T);
    
    public int MaxPoolSize { private get; init; } = 100;
    private DefaultObjectPool<T>? Pool { get; set; }
    [MemberNotNullWhen(true, nameof(Pool))] public bool UsesPool { get; private set; }
    public OmniaAssetPoolPolicy<T> PoolPolicy {
        init {
            UsesPool = true;
            
            Console.WriteLine($"Creating pool for {AssetType.Name} with size {MaxPoolSize}");
            
            Pool = new DefaultObjectPool<T>(value, MaxPoolSize);
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public TAsset CreateInstance<TAsset>(IScopedProvider scopedProvider) where TAsset : IOmniaAsset{
        // This feels incredibly weird
        if (!typeof(T).IsAssignableFrom(typeof(TAsset))) throw new InvalidOperationException("The asset type is not assignable from the registration type.");
        var asset = ActivatorHelper.CreateInstance<T>(scopedProvider);
        
        if (asset is not TAsset castedAsset) throw new InvalidOperationException("The asset is not of the expected type.");
        return castedAsset;
    }
    
    public bool TryCreateInstance<TAsset>(IScopedProvider scopedProvider, [NotNullWhen(true)] out TAsset? asset) where TAsset : IOmniaAsset {
        asset = default;

        // Create tzo different classes. One with a pool one without a pool. This wa we negate one if check
        if (UsesPool) {
            if (Pool.Get() is not TAsset obj) return false;
            asset = obj;
            return true;
        }
        
        if (!typeof(T).IsAssignableFrom(typeof(TAsset))) return false;
        if ( ActivatorHelper.CreateInstance<T>(scopedProvider) is not TAsset castedAsset) return false;
        asset = castedAsset;
        return true;
    }
}
