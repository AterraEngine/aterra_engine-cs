// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CrossData;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;
using System.Diagnostics;

namespace AterraCore.Threading.Threads.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ILogicThreadProcessor>]
public class LogicThreadProcessor(
    IWorldSpace world,
    IThreadingManager threadingManager,
    ICrossThreadDataAtlas crossThreadDataAtlas
) : AbstractThreadProcessor<LogicThreadProcessor>, ILogicThreadProcessor {

    private const int TargetTicksPerSecond = 20;// TPS
    private const double MillisecondsPerTick = 1000.0 / TargetTicksPerSecond;

    private readonly Stopwatch _tickStopwatch = Stopwatch.StartNew();
    private readonly Stopwatch _tpsStopwatch = Stopwatch.StartNew();

    private bool IsRunning { get; set; } = true;

    private int _ticks;

    // -----------------------------------------------------------------------------------------------------------------
    // Run Method
    // -----------------------------------------------------------------------------------------------------------------
    #region Run & Update
    public override void Run() {
        try {
            _tpsStopwatch.Start();

            // Game engine is actually running now
            while (IsRunning) {
                _tickStopwatch.Restart();

                // Call UPDATE LOOP
                Update();
                RunEndOfTick();

                // Wait until the end of the Tick cycle
                SleepUntilEndOfTick();
                CalculateActualTps();

                _ticks++;
                if (!CancellationToken.IsCancellationRequested) continue;

                IsRunning = false;
                break;
            }
        }
        finally {
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
        while (EndOfTickActions.TryPop(out Action? action)) {
            action();
        }

        crossThreadDataAtlas.CleanupLogicTick();// Clear for the end of the tick
    }

    private void SleepUntilEndOfTick() {
        _tickStopwatch.Stop();
        double deltaTps = _tickStopwatch.Elapsed.TotalMilliseconds;

        double sleepTime = MillisecondsPerTick - deltaTps;
        if (sleepTime > 0) Thread.Sleep((int)sleepTime);
    }

    private void CalculateActualTps() {
        if (_tpsStopwatch.ElapsedMilliseconds < 1000) return;

        crossThreadDataAtlas.DataCollector.Tps = _ticks;
        _ticks = 0;
        _tpsStopwatch.Restart();
    }
    #endregion
}
