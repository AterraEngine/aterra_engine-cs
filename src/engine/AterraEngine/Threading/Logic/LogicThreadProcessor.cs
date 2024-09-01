﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics;

namespace AterraEngine.Threading.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LogicThreadProcessor(
    ILogger logger,
    IAterraCoreWorld world,
    ILogicEventManager eventManager,
    ICrossThreadQueue crossThreadQueue,
    IThreadingManager threadingManager
) : ILogicThreadProcessor {
    private ILogger Logger { get; } = logger.ForContext<LogicThreadProcessor>();
    public CancellationToken CancellationToken { get; set; }

    private int TargetTicksPerSecond { get; set; } = 20;// TPS
    private double MillisecondsPerTick => 1000.0 / TargetTicksPerSecond;

    private bool IsRunning { get; set; } = true;
    public bool IsFinished { get; private set; }

    private readonly Stack<Action> _endOfTickActions = [];

    private readonly Stopwatch _tickStopwatch = Stopwatch.StartNew();
    private readonly Stopwatch _tpsStopwatch = Stopwatch.StartNew();
    private int _ticks;

    // -----------------------------------------------------------------------------------------------------------------
    // Run Method
    // -----------------------------------------------------------------------------------------------------------------
    #region Run & Update
    public void Run() {
        RegisterEvents();

        try {
            _tpsStopwatch.Start();

            // Game engine is actually running now
            while (IsRunning) {
                _tickStopwatch.Restart();

                // Call UPDATE LOOP
                Update();
                HandleQueue();

                // Wait until the end of the Tick cycle
                SleepUntilEndOfTick();
                CalculateActualTps();

                // End of Tick
                RunEndOfTick();
                _ticks++;
                if (!CancellationToken.IsCancellationRequested) continue;

                IsRunning = false;
                break;
            }
        }
        finally {
            IsFinished = true;
            IsRunning = false;
            threadingManager.CancelThreads();
            Logger.Information("Logic Thread Closing");
        }
    }

    private void Update() {
        if (world.ActiveLevel is not { LogicSystems: var logicSystems } activeLevel) return;

        foreach (INexitiesSystem logicSystem in logicSystems) {
            logicSystem.Tick(activeLevel);
        }
    }

    private void RunEndOfTick() {
        while (_endOfTickActions.TryPop(out Action? action)) {
            action();
        }
        _endOfTickActions.Clear();
    }

    private void SleepUntilEndOfTick() {
        _tickStopwatch.Stop();
        double deltaTps = _tickStopwatch.Elapsed.TotalMilliseconds;
        eventManager.InvokeUpdateDeltaTps(deltaTps);

        double sleepTime = MillisecondsPerTick - deltaTps;
        if (sleepTime > 0) Thread.Sleep((int)sleepTime);
    }

    private void CalculateActualTps() {
        if (_tpsStopwatch.ElapsedMilliseconds < 1000) return;

        eventManager.InvokeUpdateTps(_ticks);
        _ticks = 0;
        _tpsStopwatch.Restart();
    }
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Event Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RegisterEvents() {
        eventManager.EventChangeActiveLevel += (_, args) => _endOfTickActions.Push(() => world.TryChangeActiveLevel(args.NewLevelId));

        eventManager.EventTps += (_, d) => Logger.Debug("TPS: {0}", d);
        // eventManager.EventActualTps += (_, _) => Logger.Debug("Assets: {0}", EngineServices.GetService<IAssetInstanceAtlas>().TotalCount);
    }

    private void HandleQueue() {
        while (crossThreadQueue.TryDequeue(QueueKey.MainToLogic, out Action? action)) {
            action.Invoke();
        }
        while (crossThreadQueue.TryDequeue(QueueKey.RenderToLogic, out Action? action)) {
            action.Invoke();
        }
    }
}
