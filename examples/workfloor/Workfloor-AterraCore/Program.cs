// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts;
using AterraCore.Boot;
using AterraCore.Boot.Operations;
using AterraCore.Contracts.Boot;
using CliArgsParser;
using AterraCore.DI;
using CodeOfChaos.Extensions;

namespace Workfloor_AterraCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public async static Task Main(string[] args) {
        IEngineConfiguration config = new EngineConfiguration()
            .RegisterBootOperation<EngineConfigLoader>()
            .RegisterBootOperation<CollectDependencies>()
            .RegisterBootOperation<PluginLoaderDefine>()
            .RegisterBootOperation<PluginLoaderPreChecks>()
            .RegisterBootOperation<PluginLoaderZipImporter>()
            .RegisterBootOperation<PluginExtractor>()
            .RegisterBootOperation<CliArgsParserAssembler>()
            .RegisterBootOperation<BuildDependencies>()
        ;

        IEngine engine = config.BuildEngine();
        var argsParser = EngineServices.GetService<IArgsParser>();

        // --- Engine is running ---
        // Actually startup the engine
        if (!args.IsEmpty()) await argsParser.ParseAsyncLinear(args);
        else await engine.Run();
    }
}
