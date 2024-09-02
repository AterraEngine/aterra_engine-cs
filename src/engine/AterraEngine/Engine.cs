﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraCore.DI;
using AterraLib;
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
    IAssetAtlas assetAtlas,
    IPluginAtlas pluginAtlas,
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
        
        if (!reflectiXml.TryParse(Paths.ConfigGame, out ParsedConfigs parsedConfigs)) {
            Logger.Error("Config could not be parsed from {path}", Paths.ConfigGame);
            throw new ApplicationException("Config could not be parsed");
        }
        
        Logger.Information("Loaded {elements} reflecti xml elements", parsedConfigs.Count());
        Logger.Information("{@elements}", parsedConfigs.Count());
        
        // Now try filtering by type
        if (!parsedConfigs.TryGetConfig(AssetIdLib.AterraLib.ConfigMancer, out IAterraLibGameConfig? aterraLibConfig)) {
            Logger.Error("Config was not setup correctly. Couldn't find {key} in parsed configurations", typeof(AterraLibGameConfig));
            throw new ApplicationException("Config was not setup correctly");
        }
        
        Logger.Information("{@c}", aterraLibConfig);
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        Task<bool> logicTask = threadingManager.TrySpawnLogicThreadAsync();
        Task<bool> renderTask = threadingManager.TrySpawnRenderThreadAsync();

        await Task.WhenAll(logicTask, renderTask);
        if (!logicTask.Result) throw new ApplicationException("Failed to start LogicThread ");
        if (!renderTask.Result) throw new ApplicationException("Failed to start RenderThread ");

        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!assetAtlas.TryAssignAsset(assetRegistration, out AssetId? _)) {
                Logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }
        if (!world.TryChangeActiveLevel(AssetIdLib.AterraCore.Entities.EmptyLevel)) throw new ApplicationException("Failed to change active level");
        await Task.Delay(1_000);
        if (!world.TryChangeActiveLevel("Workfloor:Levels/DragonDucksLevel")) throw new ApplicationException("Failed to change active level to");
     
        // -------------------------------------------------------------------------------------------------------------

        // Block main thread until all sub threads have been cancelled
        WaitHandle[] waitHandles = threadingManager.GetWaitHandles();
        WaitHandle.WaitAll(waitHandles);
        Logger.Information("Child Threads have been cancelled");

        threadingManager.JoinThreads();// wait until all threads are done
        Logger.Information("Exiting AterraEngine");
    }
}
