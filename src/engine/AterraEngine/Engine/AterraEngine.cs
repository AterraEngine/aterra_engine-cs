// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Common.Attributes;
using AterraEngine.Contracts.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Engine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IAterraEngine>(ServiceLifetime.Singleton)]
public class AterraEngine : IAterraEngine {

    public async Task RunAsync() {

        // GatherDataFromGamePlugins();
    }
}
