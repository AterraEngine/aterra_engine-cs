// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Omnia;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaFactory(IScopedProvider provider) : IOmniaFactory {

    public Result<TAsset> CreateAsset<TAsset>() where TAsset : class, IOmniaAsset {
        if (provider.GetService<TAsset>() is {} serviceInstance) return serviceInstance;

        return Result<TAsset>.FromError("Failed to create asset");
    }
}
