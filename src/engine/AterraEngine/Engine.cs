// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using AterraCore.Contracts.Threading.Logic;
using AterraEngine.Threading.Render;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;
using System.Numerics;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class Engine(
    ILogger logger,
    IAssetAtlas assetAtlas,
    IAssetInstanceAtlas instanceAtlas,
    IPluginAtlas pluginAtlas,
    IAterraCoreWorld world,
    RenderThreadEvents renderThreadEvents,
    ILogicEventManager logicEventManager,
    IThreadingManager threadingManager,
    ICrossThreadQueue crossThreadQueue
) : IEngine {
    private ILogger Logger { get; } = logger.ForContext<Engine>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task Run() {
        Logger.Information("Entered AterraEngine");
        
        Task<bool> logicTask = threadingManager.TrySpawnLogicThreadAsync();
        Task<bool> renderTask = threadingManager.TrySpawnRenderThreadAsync();
        
        await Task.WhenAll(logicTask, renderTask);
        if (!logicTask.Result || !renderTask.Result) return;

        renderThreadEvents.InvokeApplicationStageChange(ApplicationStage.StartupScreen);

        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!assetAtlas.TryAssignAsset(assetRegistration, out AssetId? _)) {
                Logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }
        
        // -------------------------------------------------------------------------------------------------------------
        if(!instanceAtlas.TryGetOrCreateSingleton("Workfloor:Levels/MainLevel", out INexitiesLevel2D? level)) return;
        
        logicEventManager.InvokeStart();
        crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar("Workfloor:TextureDuckyHype"));
        crossThreadQueue.TextureRegistrarQueue.Enqueue(new TextureRegistrar("Workfloor:TextureDuckyPlatinum"));

        const int a = 50;
        for (int k = -50; k < a; k++) {
            for (int j = -50; j < a; j++) {
                // AssetId assetId = "NexitiesDebug:Entities/DuckyPlatinumActor" ;
                AssetId assetId = j % 2 == 0 ? "Workfloor:ActorDuckyHype" : "Workfloor:ActorDuckyPlatinum";
                if (!instanceAtlas.TryCreate(assetId, out IActor2D? newDucky)) continue;
                newDucky.Transform2D.Translation = new Vector2(1 * j,1 * k);
                newDucky.Transform2D.Scale = Vector2.One;
                if (!level.ChildrenIDs.TryAdd(newDucky.InstanceId)) throw new ApplicationException("Entity could not be added");
            }
        }
        if (!instanceAtlas.TryCreate(AssetIdLib.AterraCore.Entities.Camera2D, out ICamera2D? camera2D)) return;
        camera2D.RaylibCamera2D.Camera = camera2D.RaylibCamera2D.Camera with {
            Target = new Vector2(0, 0),
            Offset =  new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f),
            Rotation = 0,
            Zoom = 10
        };
        level.ChildrenIDs.TryAddFirst(camera2D.InstanceId);
        
        if (!instanceAtlas.TryCreate("Workfloor:ActorDuckyPlayer", out IPlayer2D? player2D)) return;
        player2D.Transform2D.Translation = new Vector2(5, 5);
        player2D.Transform2D.Scale = Vector2.One;
        level.ChildrenIDs.TryAddFirst(player2D.InstanceId);
        
        if (!instanceAtlas.TryCreate("Workfloor:ActorDuckyHype", out IActor2D? playerAddendum)) return;
        playerAddendum.Transform2D.Translation = new Vector2(2,2);
        playerAddendum.Transform2D.Scale = Vector2.One;
        player2D.ChildrenIDs.TryAddFirst(playerAddendum.InstanceId);
        
        if (!world.TryChangeActiveLevel("Workfloor:Levels/MainLevel")) throw new ApplicationException("Failed to change active level");
        logger.Debug("Spawned {x} entities", level.ChildrenIDs.Count);
        logger.Debug("Spawned {x} level", level.InstanceId);
        
        // -------------------------------------------------------------------------------------------------------------
        renderThreadEvents.InvokeApplicationStageChange(ApplicationStage.Level);
        
        await Task.Delay(50_000_000);
        await threadingManager.LogicThreadData!.CancellationTokenSource.CancelAsync();
        await threadingManager.RenderThreadData!.CancellationTokenSource.CancelAsync();
    }
}
