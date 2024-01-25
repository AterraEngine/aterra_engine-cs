// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using ArgsParser;
using ArgsParser.Attributes;
using AterraEngine;
using AterraEngine.EngineLoader;

namespace AterraEngine_Workfloor.CliCommands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AterraEngineCommands : CliCommandAtlas {
    
    [CliCommand<AterraEngineArgOptions>("run")]
    public void CallbackRun(AterraEngineArgOptions argOptions) {
        var engineLoader = new EngineLoader<Engine2D>(pathToEngineConfig:argOptions.EngineConfig);
        var engine = engineLoader.CreateEngine();
        engine.Run();

    }
}