// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Data;
using AterraCore.Contracts;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraLib.Contracts;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics;

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

        if (!world.TryChangeActiveLevel(StringAssetIdLib.AterraLib.Entities.EmptyLevel)) throw new ApplicationException("Failed to change active level");

        await Task.Delay(1_000);
        if (!world.TryChangeActiveLevel("Workfloor:Levels/Main")) throw new ApplicationException("Failed to change active level to");

        // -------------------------------------------------------------------------------------------------------------

        using var cts = new CancellationTokenSource();
        // Task dequeueTask = PeriodicDequeue(cts.Token);
        
        // Block main thread until all sub threads have been cancelled
        WaitHandle[] waitHandles = threadingManager.GetWaitHandles();
        WaitHandle.WaitAll(waitHandles);
        Logger.Information("Child Threads have been cancelled");
        
        
        await cts.CancelAsync();
        // await dequeueTask; // Ensure the dequeue task has completed

        threadingManager.JoinThreads();// wait until all threads are done
        Logger.Information("Exiting AterraEngine");
    }
    
    private readonly Stopwatch _tickStopwatch = Stopwatch.StartNew();
    private async Task PeriodicDequeue(CancellationToken ct) {
        while (!ct.IsCancellationRequested) {
            
            _tickStopwatch.Stop();
            double sleepTime = 1000 - _tickStopwatch.Elapsed.TotalMilliseconds;
            if (sleepTime > 0) await Task.Delay((int)sleepTime, ct);
            _tickStopwatch.Reset();
        }
    }
}
