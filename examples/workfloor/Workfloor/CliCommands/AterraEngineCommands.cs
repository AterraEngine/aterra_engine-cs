// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using OldAterraEngine.Core.Factories;
using CliArgsParser.Attributes;
using OldAterraEngine.Contracts.Engine;
using OldAterraEngine.Core;

namespace AterraEngine_Workfloor.CliCommands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AterraEngineCommands : CliCommandAtlas {
    private IEngine CreateEngine(AterraEngineArgOptions argOptions) {
        var engineLoader = new EngineFactory<Engine2D>(pathToEngineConfig:argOptions.EngineConfig);
        Engine2D? engine = engineLoader.CreateEngine();
        return engine;
    }
    
    [CliCommand<AterraEngineArgOptions>("run")]
    public bool CallbackRun(AterraEngineArgOptions argOptions) {
        IEngine? engine = CreateEngine(argOptions: argOptions);
        engine.Run();
        return true;
    }
    
    [CliCommand<AterraEngineArgOptions>("test")]
    public bool CallbackTest(AterraEngineArgOptions argOptions) {
        Console.WriteLine($"test {argOptions.EngineConfig}");
        return true;
    }
    
}