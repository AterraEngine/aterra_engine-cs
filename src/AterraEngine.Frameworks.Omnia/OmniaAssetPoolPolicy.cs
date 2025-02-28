// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaAssetPoolPolicy<TAsset>(IScopedProvider provider) : IPooledObjectPolicy<TAsset>  where TAsset : class, IOmniaAsset {
    private readonly Lazy<Func<IScopedProvider, TAsset>> _lazyFactory = new(() => ConstructorReflectionFactory.CreateFunc<TAsset>(typeof(TAsset)));
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public TAsset Create() => _lazyFactory.Value.Invoke(provider);
    public bool Return(TAsset obj) => obj.Cleanup();
}
