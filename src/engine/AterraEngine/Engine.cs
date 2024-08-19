﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.DI;
using AterraEngine.Threading;
using AterraEngine.Threading.Logic;
using AterraEngine.Threading.Render;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
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
    IApplicationStageManager applicationStageManager,
    ILogicEventManager logicEventManager
) : IEngine {
    
    
    private Thread? _renderThread;
    private readonly CancellationTokenSource _ctsRenderThread = new();
    
    private Thread? _logicThread;
    private readonly CancellationTokenSource _ctsLogicThread = new();
    
    private readonly TaskCompletionSource<bool> _openGlContextCreated = new();
    private readonly ConcurrentQueue<TextureQueueRecord> _textureQueue = new();
    private ILogger Logger { get; } = logger.ForContext<Engine>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEngine SubscribeToEvents() {
        renderThreadEvents.EventApplicationStageChange += applicationStageManager.ReceiveStageChange;
        
        renderThreadEvents.EventOpenGlContextCreated += (_, _) => _openGlContextCreated.SetResult(true);
        renderThreadEvents.EventOpenGlContextCreated += (_, _) => Logger.Information("OpenGL Context Created");
        
        return this;
    }

    public IEngine SpawnRenderThread() {
        var renderThreadProcessor = EngineServices.CreateWithServices<RenderThreadProcessor>();
        renderThreadProcessor.CancellationToken = _ctsRenderThread.Token;
        renderThreadProcessor.TextureQueue = _textureQueue;
        _renderThread = new Thread(renderThreadProcessor.Run);
        _renderThread.Start();
        Logger.Information("Spawned RenderThread");

        return this;
    }

    public IEngine SpawnLogicThread() {
        var logicThreadProcessor = EngineServices.CreateWithServices<LogicThreadProcessor>();
        logicThreadProcessor.CancellationToken = _ctsLogicThread.Token;
        logicThreadProcessor.RegisterEvents();
        
        _logicThread = new Thread(logicThreadProcessor.Run);
        _logicThread.Start();
        Logger.Information("Spawned LogicThread");

        return this;
    }

    public async Task Run() {
        Logger.Information("Entered AterraEngine");

        // Don't wait forever for the opengl context to be created, else we will have many issues
        try {
            var cts = new CancellationTokenSource(50000);
            await _openGlContextCreated.Task.WithCancellation(cts.Token);
        }
        catch (OperationCanceledException ex) {
            Logger.Fatal(ex, "Render Thread did not signal back of successful window creation before the timeout period");
            await HandleFatalExceptionGracefully();
        }

        renderThreadEvents.InvokeApplicationStageChange(ApplicationStage.StartupScreen);

        foreach (AssetRegistration assetRegistration in pluginAtlas.GetAssetRegistrations()) {
            if (!assetAtlas.TryAssignAsset(assetRegistration, out AssetId? _)) {
                Logger.Warning("Type {Type} could not be assigned as an asset", assetRegistration.Type);
            }
        }
        
        // -------------------------------------------------------------------------------------------------------------
        if(!instanceAtlas.TryGetOrCreateSingleton("Workfloor:Levels/MainLevel", out INexitiesLevel2D? level)) return;
        
        logicEventManager.InvokeStart();
        
        _textureQueue.Enqueue(new TextureQueueRecord (
            TextureAssetId :  "Workfloor:TextureDuckyHype"
        ));
        _textureQueue.Enqueue(new TextureQueueRecord (
            TextureAssetId :  "Workfloor:TextureDuckyPlatinum"
        ));

        const int a = 100;
        for (int k = 0; k < a; k++) {
            for (int j = 0; j < a; j++) {
                // AssetId assetId = "NexitiesDebug:Entities/DuckyPlatinumActor" ;
                AssetId assetId = j % 2 == 0 ? "Workfloor:ActorDuckyHype" : "Workfloor:ActorDuckyPlatinum";
                if (!instanceAtlas.TryCreate(assetId, out IActor2D? newDucky)) continue;
                newDucky.Transform2D.Translation = new Vector2(50 * j,50 * k);
                newDucky.Transform2D.Scale = new Vector2(50, 50);
                if (!level.ChildrenIDs.TryAdd(newDucky.InstanceId)) throw new ApplicationException("Entity could not be added");
            }
        }
        
        if (!instanceAtlas.TryCreate("Workfloor:ActorDuckyPlayer", out IPlayer2D? player2D)) return;
        player2D.Transform2D.Translation = new Vector2(250, 250);
        player2D.Transform2D.Scale = new Vector2(50, 50);
        level.ChildrenIDs.TryAddFirst(player2D.InstanceId);
        
        if (!instanceAtlas.TryCreate("Workfloor:ActorDuckyHype", out IActor2D? playerAddendum)) return;
        playerAddendum.Transform2D.Translation = new Vector2(10,10);
        playerAddendum.Transform2D.Scale = new Vector2(1, 1);
        player2D.ChildrenIDs.TryAddFirst(playerAddendum.InstanceId);
        
        if (!instanceAtlas.TryCreate("Workfloor:ActorDuckyHype", out IActor2D? playerAddendum2)) return;
        playerAddendum.Transform2D.Translation = new Vector2(10,10);
        playerAddendum.Transform2D.Scale = new Vector2(1, 1);
        playerAddendum.ChildrenIDs.TryAddFirst(playerAddendum2.InstanceId);
        
        if (!world.TryChangeActiveLevel("Workfloor:Levels/MainLevel")) throw new ApplicationException("Failed to change active level");
        logger.Debug("Spawned {x} entities", level.ChildrenIDs.Count);
        logger.Debug("Spawned {x} level", level.InstanceId);
        
        // -------------------------------------------------------------------------------------------------------------
        renderThreadEvents.InvokeApplicationStageChange(ApplicationStage.Level);
        
        await Task.Delay(50_000_000);
        await _ctsRenderThread.CancelAsync();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private async Task HandleFatalExceptionGracefully() {
        await _ctsRenderThread.CancelAsync();
        Logger.ExitFatal(-1, "Fatally Crashing gracefully");
    }
}
