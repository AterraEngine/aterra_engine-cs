// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts;
using AterraCore.Boot;
using AterraCore.Boot.Operations;
using AterraCore.Contracts.Boot;
using AterraLib;

namespace Workfloor_AterraCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main(string[] args) {
        IEngineConfiguration config = new EngineConfiguration()
            .RegisterBootOperation<EngineConfigLoader>()
            .RegisterBootOperation<RegisterAssemblyAsPlugin<AterraLibEntry>>()
            .RegisterBootOperation<CollectDefaultDependencies>()
            .RegisterBootOperation<PluginLoaderDefine>()
            .RegisterBootOperation<PluginLoaderPreChecks>()
            .RegisterBootOperation<PluginLoaderZipImporter>()
            .RegisterBootOperation<PluginExtractor>()
            .RegisterBootOperation<BuildDependencies>()
        ;

        IEngine engine = config.BuildEngine();
        
        // --- Engine is running ---
        // Actually startup the engine
        Task.Run(engine.Run)
            .GetAwaiter()
            .GetResult();
    }
}