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
using AterraCore.Contracts.Threading;
using JetBrains.Annotations;
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
    IThreadingManager threadingManager
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
        if (!logicTask.Result) throw new ApplicationException("Failed to start LogicThread ");
        if (!renderTask.Result) throw new ApplicationException("Failed to start RenderThread ");

        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!assetAtlas.TryAssignAsset(assetRegistration, out AssetId? _)) {
                Logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }
        if (!world.TryChangeActiveLevel(AssetIdLib.AterraCore.Entities.EmptyLevel)) throw new ApplicationException("Failed to change active level");
        await Task.Delay(5_000);

        // -------------------------------------------------------------------------------------------------------------
        // const int levelGenerations = int.MaxValue;
        // const int entitiesPerLevel = 1_000_000;
        const int entitiesPerLevel = 10_000;
        const int levelGenerations = 1;
        for (int levelI = 0; levelI < levelGenerations; levelI++) {
            var levelInstanceId = Ulid.NewUlid();
            Logger.Information("level id {id}", levelInstanceId);
            if (!instanceAtlas.TryGetOrCreate("Workfloor:Levels/MainLevel", levelInstanceId, out INexitiesLevel2D? level)) return;


            int a = (int)(Math.Sqrt(entitiesPerLevel) / 2f);
            int i = levelI + 1;
            Parallel.For(-a, a, body: k => {
                Parallel.For(-a, a, body: j => {
                    if (!instanceAtlas.TryCreate(j % 2 == 0 ? "Workfloor:ActorDuckyHype" : "Workfloor:ActorDuckyPlatinum", out IActor2D? newDucky)) return;
                    newDucky.Transform2D.Translation = new Vector2(i * j, i * k);
                    newDucky.Transform2D.Scale = Vector2.One;
                    if (!level.ChildrenIDs.TryAdd(newDucky.InstanceId)) throw new ApplicationException("Entity could not be added");
                });
            });

            if (!instanceAtlas.TryCreate(AssetIdLib.AterraCore.Entities.Camera2D, out ICamera2D? camera2D)) continue;
            camera2D.RaylibCamera2D.Camera = camera2D.RaylibCamera2D.Camera with {
                Target = new Vector2(0, 0),
                Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f),
                Rotation = 0,
                Zoom = 10
            };
            level.ChildrenIDs.TryAddFirst(camera2D.InstanceId);

            if (!instanceAtlas.TryCreate("Workfloor:ActorDuckyPlayer", out IPlayer2D? player2D)) continue;
            player2D.Transform2D.Translation = new Vector2(5, 5);
            player2D.Transform2D.Scale = Vector2.One;
            level.ChildrenIDs.TryAddFirst(player2D.InstanceId);

            if (!instanceAtlas.TryCreate("Workfloor:ActorDuckyHype", out IActor2D? playerAddendum)) continue;
            playerAddendum.Transform2D.Translation = new Vector2(2, 2);
            playerAddendum.Transform2D.Scale = Vector2.One;
            player2D.ChildrenIDs.TryAddFirst(playerAddendum.InstanceId);

            // await Task.Delay(1_000);
            if (!world.TryChangeActiveLevel("Workfloor:Levels/MainLevel", level.InstanceId)) throw new ApplicationException($"Failed to change active level to {level.InstanceId}");
        }
        // -------------------------------------------------------------------------------------------------------------

        // Block main thread until all sub threads have been cancelled
        WaitHandle[] waitHandles = threadingManager.GetWaitHandles();
        WaitHandle.WaitAll(waitHandles);
        Logger.Information("Child Threads have been cancelled");

        threadingManager.JoinThreads();// wait until all threads are done
        Logger.Information("Exiting AterraEngine");
    }
}
