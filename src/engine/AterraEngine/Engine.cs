// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Contracts;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraCore.DI;
using AterraLib.Contracts;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class Engine(
    ILogger logger,
    IAterraCoreWorld world,
    IThreadingManager threadingManager
) : IEngine {
    private ILogger Logger { get; } = logger.ForContext<Engine>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task Run() {
        Logger.Information("Entered AterraEngine");


        var reflectiXml = EngineServices.GetService<IConfigMancerParser>();
        
        if (!reflectiXml.TryParseGameConfig(Paths.ConfigGame, out ParsedConfigs parsedConfigs)) {
            Logger.Error("Config could not be parsed from {path}", Paths.ConfigGame);
            throw new ApplicationException("Config could not be parsed");
        }
        
        Logger.Information("Loaded {elements} reflecti xml elements", parsedConfigs.Count);
        Logger.Information("{@elements}", parsedConfigs.Count);
        
        // Now try filtering by type
        if (!parsedConfigs.TryGetConfig(out IAterraLibGameConfig? aterraLibConfig)) {
            throw new ApplicationException("Config was not setup correctly");
        }
        
        Logger.Information("{@c}", aterraLibConfig);
        
        
        Task<bool> logicTask = threadingManager.TrySpawnLogicThreadAsync();
        Task<bool> renderTask = threadingManager.TrySpawnRenderThreadAsync();

        await Task.WhenAll(logicTask, renderTask);
        if (!logicTask.Result) throw new ApplicationException("Failed to start LogicThread ");
        if (!renderTask.Result) throw new ApplicationException("Failed to start RenderThread ");
        
        if (!world.TryChangeActiveLevel(AssetIdLib.AterraCore.Entities.EmptyLevel)) throw new ApplicationException("Failed to change active level");
        await Task.Delay(1_000);
        if (!world.TryChangeActiveLevel("Workfloor:Levels/MainLevel")) throw new ApplicationException("Failed to change active level to");
     
        // -------------------------------------------------------------------------------------------------------------

        // Block main thread until all sub threads have been cancelled
        WaitHandle[] waitHandles = threadingManager.GetWaitHandles();
        WaitHandle.WaitAll(waitHandles);
        Logger.Information("Child Threads have been cancelled");

        threadingManager.JoinThreads();// wait until all threads are done
        Logger.Information("Exiting AterraEngine");
    }
}
