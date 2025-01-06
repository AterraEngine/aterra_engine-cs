// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaFactory : IOmniaFactory {
    
    public Result<TAsset> CreateAsset<TAsset>() where TAsset : IOmniaAsset {
        ServiceProvider provider = new ServiceCollection().BuildServiceProvider();
        
        if (provider.GetService<TAsset>() is {} serviceInstance) return serviceInstance;
        if (ActivatorUtilities.CreateInstance<TAsset>(provider) is {} instance) return instance;

        return Result<TAsset>.FromError("Failed to create asset");
    }
}
