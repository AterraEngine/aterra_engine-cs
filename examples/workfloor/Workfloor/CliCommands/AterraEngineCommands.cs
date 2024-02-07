// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Engine;
using AterraEngine.Core;
using AterraEngine.Core.Factories;
using CliArgsParser.Attributes;

namespace AterraEngine_Workfloor.CliCommands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AterraEngineCommands : CliCommandAtlas {
    private IEngine CreateEngine(AterraEngineArgOptions argOptions) {
        var engineLoader = new EngineFactory<Engine2D>(pathToEngineConfig:argOptions.EngineConfig);
        var engine = engineLoader.CreateEngine();
        return engine;
    }
    
    [CliCommand<AterraEngineArgOptions>("run")]
    public bool CallbackRun(AterraEngineArgOptions argOptions) {
        var engine = CreateEngine(argOptions: argOptions);
        engine.Run();
        return true;
    }
    
    [CliCommand<AterraEngineArgOptions>("test")]
    public bool CallbackTest(AterraEngineArgOptions argOptions) {
        Console.WriteLine($"test {argOptions.EngineConfig}");
        return true;
    }
    
}