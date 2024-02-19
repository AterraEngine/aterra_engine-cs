// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts;
using AterraEngine.Core.Startup;
using AterraEngine.Core.Types;
using CliArgsParser;
using CliArgsParser.Attributes;
using Workfloor.Data;

namespace Workfloor.Cli;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class CommandAtlas: CliCommandAtlas {

    private static IAterraEngine CreateEngine(AterraEngineArgs args) {
        var factory = EngineFactory.CreateFromConfigFile(args.ConfigFile, new WorkfloorPlugin(new PluginId(0)));
        return factory.CreateEngine();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------

    [CliCommand<AterraEngineArgs>("run")]
    public void Run(AterraEngineArgs args) {
        var engine = CreateEngine(args);
        engine.Run();
    }
}