// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Core.DependencyInjection;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineServices {
    public static IServiceProvider Provider { get; private set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static void ConfigureServices(IServiceCollection collection, IEnumerable<ServiceDescriptor> serviceDescriptors) {
        foreach (ServiceDescriptor descriptor in serviceDescriptors) {
            collection.Add(descriptor);
        }

        Provider = collection.BuildServiceProvider();
    }
}
