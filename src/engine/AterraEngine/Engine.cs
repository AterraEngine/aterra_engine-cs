﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Data;
using AterraCore.Contracts;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraLib.Contracts;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IEngine, Engine>]
public class Engine(
    ILogger logger,
    IAterraCoreWorld world,
    IConfigAtlas configAtlas,
    IThreadingManager threadingManager
) : IEngine {
    private ILogger Logger { get; } = logger.ForContext<Engine>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task Run() {
        Logger.Information("Entered AterraEngine");
        
        if (!configAtlas.GameConfigs.TryGetConfig(out IAterraLibGameConfig? aterraLibConfig)) {
            throw new ApplicationException("Config was not setup correctly");
        }
        Logger.Information("{@c}", aterraLibConfig);
        
        
        Task<bool> logicTask = threadingManager.TrySpawnLogicThreadAsync();
        Task<bool> renderTask = threadingManager.TrySpawnRenderThreadAsync();
        
        await Task.WhenAll(logicTask, renderTask);
        if (!logicTask.Result) throw new ApplicationException("Failed to start LogicThread ");
        if (!renderTask.Result) throw new ApplicationException("Failed to start RenderThread ");
        
        if (!world.TryChangeActiveLevel(AssetIdStringLib.AterraLib.Entities.EmptyLevel)) throw new ApplicationException("Failed to change active level");
        await Task.Delay(1_000);
        if (!world.TryChangeActiveLevel("Workfloor:Levels/Main")) throw new ApplicationException("Failed to change active level to");
        
        // -------------------------------------------------------------------------------------------------------------
        
        // Block main thread until all sub threads have been cancelled
        WaitHandle[] waitHandles = threadingManager.GetWaitHandles();
        WaitHandle.WaitAll(waitHandles);
        Logger.Information("Child Threads have been cancelled");
        
        threadingManager.JoinThreads();// wait until all threads are done
        Logger.Information("Exiting AterraEngine");
    }
}
