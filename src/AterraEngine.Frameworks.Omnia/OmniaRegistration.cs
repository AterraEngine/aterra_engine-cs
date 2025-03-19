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

    [MemberNotNullWhen(true, nameof(PoolPolicyBuilder))] public bool UsesPool => PoolPolicyBuilder is not null;
    public Func<IScopedProvider, OmniaAssetPoolPolicy<T>>? PoolPolicyBuilder { get ; init; }
    private DefaultObjectPool<T>? _pool;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public DefaultObjectPool<T>? GetPool(IScopedProvider scopedProvider) {
        if (!UsesPool || _pool is not null) return _pool;
        return _pool = new DefaultObjectPool<T>(
            PoolPolicyBuilder(scopedProvider),
            MaxPoolSize
        );
    }

    public void ReturnToPool<TAsset>(TAsset instance) {
        if (instance is not T casted) return;
        _pool?.Return(casted);
    }

    public TAsset CreateInstance<TAsset>(IScopedProvider scopedProvider) where TAsset : IOmniaAsset{
        // This feels incredibly weird
        if (!typeof(T).IsAssignableFrom(typeof(TAsset))) throw new InvalidOperationException("The asset type is not assignable from the registration type.");
        var asset = scopedProvider.GetRequiredService<T>();
        
        if (asset is not TAsset castedAsset) throw new InvalidOperationException("The asset is not of the expected type.");
        return castedAsset;
    }
    
    public bool TryCreateInstance<TAsset>(IScopedProvider scopedProvider, [NotNullWhen(true)] out TAsset? asset) where TAsset : IOmniaAsset {
        asset = default;

        // Create tzo different classes. One with a pool one without a pool. This wa we negate one if check
        if (UsesPool && GetPool(scopedProvider) is { } pool) {
            if (pool.Get() is not TAsset obj) return false;
            asset = obj;
            return true;
        }
        
        if (!typeof(T).IsAssignableFrom(typeof(TAsset))) return false;
        if (scopedProvider.GetRequiredService<T>() is not TAsset castedAsset) return false;
        asset = castedAsset;
        return true;
    }
    
    public IOmniaRegistrationKey GetLookupKey() 
        => new OmniaRegistrationKey(
            OmniaId,
            AssetType
        );
}
