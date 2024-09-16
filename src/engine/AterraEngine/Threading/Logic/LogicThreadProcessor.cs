// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics;

namespace AterraEngine.Threading.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ILogicThreadProcessor>]
public class LogicThreadProcessor(
    ILogger logger,
    IAterraCoreWorld world,
    ILogicEventManager eventManager,
    ICrossThreadEventManager crossThreadEventManager,
    IThreadingManager threadingManager
) : AbstractThreadProcessor, ILogicThreadProcessor {

    private const int TargetTicksPerSecond = 20;// TPS
    private const double MillisecondsPerTick = 1000.0 / TargetTicksPerSecond;
    
    private readonly Stopwatch _tickStopwatch = Stopwatch.StartNew();
    private readonly Stopwatch _tpsStopwatch = Stopwatch.StartNew();
    private int _ticks;
    private ILogger Logger { get; } = logger.ForContext<LogicThreadProcessor>();

    private bool IsRunning { get; set; } = true;
    
    public event TickEventHandler? TickEventLogic;  
    public event EmptyEventHandler? TickEventClearCaches;  

    // -----------------------------------------------------------------------------------------------------------------
    // Event Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void RegisterEventsStartup() {
        eventManager.EventTps += (_, d) => Logger.Debug("TPS: {0}", d);
    }
    
    public override void OnLevelChangeStarted(IActiveLevel oldLevel) {
        TickEventLogic = null;
        TickEventClearCaches = null;
    }
    
    public override void OnLevelChangeCompleted(IActiveLevel newLevel) {
        RegisterTickEvents<ILogicSystem>(ref TickEventLogic, newLevel.TryGetLogicSystems, system => system.LogicTick);
        RegisterClearCacheEvents<ILogicClearableCacheSystem>(ref TickEventClearCaches, newLevel.TryGetLogicClearableCacheSystems, system => system.LogicThreadClearCaches);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Run Method
    // -----------------------------------------------------------------------------------------------------------------
    #region Run & Update
    public override void Run() {
        RegisterEventsStartup();

        try {
            _tpsStopwatch.Start();

            // Game engine is actually running now
            while (IsRunning) {
                _tickStopwatch.Restart();

                // Call UPDATE LOOP
                if (world.ActiveLevel is {} activeLevel) TickEventLogic?.Invoke(activeLevel);
                crossThreadEventManager.InvokeLogicTickCleanup();

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
}
