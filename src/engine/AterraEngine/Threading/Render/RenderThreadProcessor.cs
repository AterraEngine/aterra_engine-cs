﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading.Rendering;
using JetBrains.Annotations;
using Serilog;
using static Raylib_cs.Raylib;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class RenderThreadProcessor(
    ILogger logger,
    IMainWindow mainWindow,
    IAterraCoreWorld world,
    ITextureAtlas textureAtlas,
    ICrossThreadQueue crossThreadQueue,
    IThreadingManager threadingManager,
    IDataCollector dataCollector,
    IRenderEventManager eventManager,
    ILogicEventManager logicEventManager,
    IAssetInstanceAtlas instanceAtlas
) :  IRenderThreadProcessor {
    private ILogger Logger { get; } = logger.ForContext<RenderThreadProcessor>();
    public CancellationToken CancellationToken { get; set; }
    
    private static Color ClearColor { get; } = new(0, 0, 0, 0);
    private readonly Stack<Action> _endOfTickActions = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run() {
        mainWindow.Init();
        
        try {
            // Window is actually running now
            while (!WindowShouldClose()) {
                Update();
                HandleQueue();
            
                // End of Tick
                #region End Of Tick
                while (_endOfTickActions.TryPop(out Action? action)) {
                    action();
                }
                _endOfTickActions.Clear();
                if (CancellationToken.IsCancellationRequested) break;
                #endregion
            }
        }
        finally {
            Logger.Information("Render Thread Closing");
            CloseWindow();
            threadingManager.CancelThreads();
        }
    }
    
    private void Update() {
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        
        BeginDrawing();
        ClearBackground(ClearColor);

        if (level.Camera2DEntity is { Camera: var camera2D }) {
            BeginMode2D(camera2D);
            foreach (INexitiesSystem system in level.RenderSystems) 
                system.Tick(level);
            EndMode2D();
        }
        
        DrawUi(level);
        EndDrawing();
        
        logicEventManager.InvokeUpdateFps(GetFPS());
    }
    
    private void DrawUi(IActiveLevel level) {
        DrawRectangle(0, 0, 250, 50 * 9, Color.White);
        DrawText($"   FPS : {dataCollector.Fps}", 0, 0, 32, Color.DarkBlue);
        DrawText($"minFPS : {dataCollector.FpsMin}", 0, 50, 32, Color.DarkBlue);
        DrawText($"maxFPS : {dataCollector.FpsMax}", 0, 100, 32, Color.DarkBlue);
        DrawText($"avgFPS : {dataCollector.FpsAverage:N2}", 0, 150, 32, Color.DarkBlue);
        DrawText($"   TPS : {dataCollector.Tps}", 0, 200, 32, Color.DarkBlue);
        DrawText($"avgTPS : {dataCollector.TpsAverage:N2}", 0, 250, 32, Color.DarkBlue);
        
        DrawText($"DUCKS : {level.RawLevelData.ChildrenIDs.Count}",0, 300, 32, Color.DarkBlue);
        DrawText($"entities : {instanceAtlas.TotalCount}",0, 350, 32, Color.DarkBlue);
        DrawText($"ID : {level.RawLevelData.AssetId}",0, 400, 12, Color.DarkBlue);
    }
    
    public void RegisterEvents() {
        eventManager.EventClearSystemCaches += OnEventManagerOnEventClearSystemCaches;
    }
    
    private void OnEventManagerOnEventClearSystemCaches(object? _, EventArgs __) {
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        foreach (INexitiesSystem system in level.RenderSystems) 
            system.InvalidateCaches();
    }
    
    private void HandleQueue() {
        while (crossThreadQueue.TextureRegistrarQueue.TryDequeue(out TextureRegistrar? textureRecord)) {
            if (textureRecord.UnRegister) PushUnRegisterTexture(textureRecord);
            PushRegisterTexture(textureRecord);
        }
        
        while (crossThreadQueue.TryDequeue(QueueKey.LogicToRender, out Action? action)) _endOfTickActions.Push(action); 
        while (crossThreadQueue.TryDequeue(QueueKey.MainToRender, out Action? action)) _endOfTickActions.Push(action); 
    }
    
    private void PushRegisterTexture(TextureRegistrar textureRecord) {
        _endOfTickActions.Push(() => {
             textureAtlas.TryRegisterTexture(textureRecord.TextureAssetId);
             eventManager.InvokeClearSystemCaches();
        });
    }
    
    private void PushUnRegisterTexture(TextureRegistrar textureRecord) {
        _endOfTickActions.Push(() => {
            textureAtlas.TryUnRegisterTexture(textureRecord.TextureAssetId);
            eventManager.InvokeClearSystemCaches();
        });
    }
}
