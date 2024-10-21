// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Builder;
using AterraEngine.Builder.BootOperations;
using AterraEngine.Builder.BootOperations.Chains.PluginExtraction;
using AterraEngine.Contracts.Builder.BootOperations;
using AterraEngine.Contracts.Engine;
using System.Diagnostics;

namespace Workfloor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public async static Task Main(string[] args) {
        // -------------------------------------------------------------------------------------------------------------
        // Builder
        // -------------------------------------------------------------------------------------------------------------
        var builder = new AterraEngineBuilder();
        
        builder.WithPlugins(config => {
            config.PluginPath = "plugins/";
            config.IsPresent("AterraEngine.Core");
        });
        
        // -------------------------------------------------------------------------------------------------------------
        // Engine
        // -------------------------------------------------------------------------------------------------------------
        IAterraEngine engine = await builder.BuildAsync();
        
        await engine.RunAsync();
    }
}
