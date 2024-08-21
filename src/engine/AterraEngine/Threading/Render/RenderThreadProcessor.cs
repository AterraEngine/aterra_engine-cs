// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.DataCollector;
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Renderer;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading.Rendering;
using CodeOfChaos.Extensions.Serilog;
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
    ILogicEventManager logicEventManager
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
                RunEndOfTick();
                if (CancellationToken.IsCancellationRequested) break;
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
    

    private void RunEndOfTick() {
        while (_endOfTickActions.TryPop(out Action? action)) {
            action();
        }
        _endOfTickActions.Clear();
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
        DrawText($"ID : {level.RawLevelData.AssetId}",0, 350, 12, Color.DarkBlue);
    }
    
    public void RegisterEvents() {
        eventManager.EventStart += Start;
        eventManager.EventStop += Stop;
        eventManager.EventClearSystemCaches += OnEventManagerOnEventClearSystemCaches;
    }
    
    private void OnEventManagerOnEventClearSystemCaches(object? _, EventArgs __) {
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return;
        foreach (INexitiesSystem system in level.RenderSystems) 
            system.ClearCaches();
    }

    private void Start(object? _, EventArgs __) {
        Logger.Information("Thread started");
    }

    private void Stop(object? _, EventArgs __) {
        Logger.Information("Thread stopped");
    }
    
    private void HandleQueue() {
        while (crossThreadQueue.TextureRegistrarQueue.TryDequeue(out TextureRegistrar? textureRecord)) {
            // if (textureRecord.UnRegister) textureAtlas.TryUnRegisterTexture(textureRecord.TextureAssetId);
            _endOfTickActions.Push(() => {
                textureAtlas.TryRegisterTexture(textureRecord.TextureAssetId);
                eventManager.InvokeClearSystemCaches();
            });
           
        }
        
        while (crossThreadQueue.TryDequeue(QueueKey.LogicToRender, out Action? action)) _endOfTickActions.Push(action); 
        while (crossThreadQueue.TryDequeue(QueueKey.MainToRender, out Action? action)) _endOfTickActions.Push(action); 
    }
}
