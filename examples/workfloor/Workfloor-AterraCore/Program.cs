// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot;
using AterraCore.Boot.Operations;
using AterraCore.Contracts;
using CodeOfChaos.Extensions;

namespace Workfloor_AterraCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public async static Task Main(string[] args) {
        var builder = new EngineBuilder();
        
        builder.RegisterBootOperations(config => {
            config.AddOperation<EngineConfigLoader>();
            config.AddOperation<CollectDependenciesByAttribute>(); // Do these first, because the manual ones might overwrite
            config.AddOperation<CollectDependenciesManually>();
            config.AddOperation<PluginLoaderDefine>();
            config.AddOperation<PluginLoaderPreChecks>();
            config.AddOperation<PluginLoaderZipImporter>();
            config.AddOperation<PluginExtractor>();
            config.AddOperation<CliArgsParserAssembler > ();
            config.AddOperation<BuildDependencies>();
        });
        
        IEngine engine = builder.BuildEngine();
        
        // --- Engine is running ---
        // Actually startup the engine
        if (!args.IsEmpty())
            await engine.GetArgsParser().ParseAsyncLinear(args);
        else
            await engine.RunAsync();
    }
}
