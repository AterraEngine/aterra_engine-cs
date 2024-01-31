// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Engine;
using AterraEngine.Core;
using AterraEngine.Core.Factories;
using CliCommandParser;
using CliCommandParser.Attributes;

namespace AterraEngine_Workfloor.CliCommands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AterraEngineCommands : CliCommandAtlas {
    
    [CliCommand<AterraEngineArgOptions>("run")]
    public void CallbackRun(AterraEngineArgOptions argOptions) {
        var engine = CreateEngine(argOptions: argOptions);
        engine.Run();

    }
    
    [CliCommand<AterraEngineArgOptions>("test")]
    public void CallbackTest(AterraEngineArgOptions argOptions) {
        Console.WriteLine($"test {argOptions.EngineConfig}");
    }

    private IEngine CreateEngine(AterraEngineArgOptions argOptions) {
        var engineLoader = new EngineFactory<Engine2D>(pathToEngineConfig:argOptions.EngineConfig);
        var engine = engineLoader.CreateEngine();
        return engine;
    }
    
}