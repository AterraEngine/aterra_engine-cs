﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.Contracts.Threading.CrossThread.Dto;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading.Rendering;
using JetBrains.Annotations;
using Serilog;
using System.Numerics;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IRenderThreadProcessor>]
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
) : IRenderThreadProcessor {
    private ILogger Logger { get; } = logger.ForContext<RenderThreadProcessor>();
    public CancellationToken CancellationToken { get; set; }

    private static Color ClearColor { get; } = new(0, 0, 0, 0);
    private readonly Stack<Action> _endOfFrameActions = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool WhileCondition => !Raylib.WindowShouldClose()
        || CancellationToken.IsCancellationRequested;
    public void Run() {
        RegisterEvents();
        mainWindow.Init();

        // Window is actually running now
        while (WhileCondition) {
            logicEventManager.InvokeUpdateFps(Raylib.GetFPS());
            Update();
            HandleQueue();

            while (_endOfFrameActions.TryPop(out Action? action))
                action();
        }

        Logger.Information("Render Thread Closing");
        Raylib.CloseWindow();
        threadingManager.CancelThreads();
    }

    private void Update() {
        if (world.ActiveLevel is not {
                Camera2DEntity: var camera2DEntity,
                RenderSystems: var renderSystems
                // UiSystems: var uiSystems
            } activeLevel) return;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(ClearColor);

        if (camera2DEntity is { Camera: var camera2D }) {
            Raylib.BeginMode2D(camera2D);
            foreach (INexitiesSystem system in renderSystems) {
                system.Tick(activeLevel);
            }
            Raylib.EndMode2D();
        }

        DrawUi(activeLevel);
        Raylib.EndDrawing();

        if (Raylib.IsWindowResized()) eventManager.InvokeWindowResized();
    }

    private void DrawUi(ActiveLevel level) {
        Raylib.DrawRectangle(0, 0, 250, 50 * 9, new Color(0, 0, 0, 127));

        Raylib.DrawText($"   FPS : {dataCollector.Fps}", 0, 0, 32, Color.LightGray);
        Raylib.DrawText($"minFPS : {dataCollector.FpsMin}", 0, 50, 32, Color.LightGray);
        Raylib.DrawText($"maxFPS : {dataCollector.FpsMax}", 0, 100, 32, Color.LightGray);
        Raylib.DrawText($"avgFPS : {dataCollector.FpsAverageString}", 0, 150, 32, Color.LightGray);
        Raylib.DrawText($"   TPS : {dataCollector.Tps}", 0, 200, 32, Color.LightGray);
        Raylib.DrawText($"avgTPS : {dataCollector.TpsAverageString}", 0, 250, 32, Color.LightGray);
        Raylib.DrawText($" DUCKS : {level.RawLevelData.ChildrenIDs.Count}", 0, 300, 32, Color.LightGray);
        Raylib.DrawText($"entGlb : {instanceAtlas.TotalCount}", 0, 350, 32, Color.LightGray);
        Raylib.DrawText($" Asset : {level.RawLevelData.AssetId}", 0, 400, 12, Color.LightGray);
        Raylib.DrawText($"  Inst : {level.RawLevelData.InstanceId}", 0, 425, 12, Color.LightGray);
    }

    public void RegisterEvents() {
        eventManager.EventClearSystemCaches += (_, _) => _endOfFrameActions.Push(OnEventManagerOnEventClearSystemCaches);
        eventManager.EventWindowResized += (_, _) => _endOfFrameActions.Push(OnEventManagerOnEventWindowResized);
    }

    private void HandleQueue() {
        while (crossThreadQueue.TextureRegistrarQueue.TryDequeue(out TextureRegistrar? textureRecord)) {
            if (textureRecord.UnRegister) PushUnRegisterTexture(textureRecord);
            else PushRegisterTexture(textureRecord);
        }
        if (crossThreadQueue.EntireQueueIsEmpty) return;

        while (crossThreadQueue.TryDequeue(QueueKey.LogicToRender, out Action? action)) _endOfFrameActions.Push(action);
        while (crossThreadQueue.TryDequeue(QueueKey.MainToRender, out Action? action)) _endOfFrameActions.Push(action);
    }

    private void PushRegisterTexture(TextureRegistrar textureRecord) {
        _endOfFrameActions.Push(RegisterTexture);
        return;

        void RegisterTexture() {
            textureAtlas.TryRegisterTexture(textureRecord.TextureAssetId);
            eventManager.InvokeClearSystemCaches();
        }
    }

    private void PushUnRegisterTexture(TextureRegistrar textureRecord) {
        _endOfFrameActions.Push(UnregisterTexture);
        return;

        void UnregisterTexture() {
            textureAtlas.TryUnRegisterTexture(textureRecord.TextureAssetId);
            eventManager.InvokeClearSystemCaches();
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Events
    // -----------------------------------------------------------------------------------------------------------------
    private void OnEventManagerOnEventClearSystemCaches() {
        if (world.ActiveLevel is not { RenderSystems: var renderSystems }) return;
        foreach (INexitiesSystem system in renderSystems) {
            system.InvalidateCaches();
        }
    }

    private void OnEventManagerOnEventWindowResized() {
        if (world.ActiveLevel is not { Camera2DEntity: {} camera2DEntity }) return;
        camera2DEntity.Camera = camera2DEntity.Camera with {
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f)
        };
    }
}
