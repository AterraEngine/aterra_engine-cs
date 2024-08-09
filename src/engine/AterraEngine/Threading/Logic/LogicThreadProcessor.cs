// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.OmniVault.Textures;
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
    INexitiesWorld world,
    ILogicEventManager eventManager
) : AbstractThread {
    private ILogger Logger { get; } = logger.ForContext<LogicThreadProcessor>();
    
    public int TargetTicksPerSecond { get; set; } = 20; // TPS
    private double MillisecondsPerTick => 1000.0 / TargetTicksPerSecond;
    public bool IsStarted { get; private set; }
    public bool IsRunning { get; private set; } = true;
    public bool IsFinished { get; private set; }
    public double ActualTps { get; private set; }
    
    public List<Action> EndOfTickActions { get; set; } = [];

    private Stopwatch TickStopwatch { get; } = Stopwatch.StartNew();
    private Stopwatch TpsStopwatch { get; } = Stopwatch.StartNew();
    private int _ticks = 0;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Run Method
    // -----------------------------------------------------------------------------------------------------------------
    #region Run & Update
    public override void Run() {
        // Wait until the main thread signaled to start
        while (!IsStarted) {
            TickStopwatch.Restart();
            SleepUntilEndOfTick();
        }
        
        TpsStopwatch.Start();

        // Game engine is actually running now
        while (IsRunning) {
            TickStopwatch.Restart();
            
            // Call UPDATE LOOP
            Update();
            RunEndOfTick();
            
            // Wait until the end of the Tick cycle
            SleepUntilEndOfTick();
            CalculateActualTps();
            
            // End of Tick
            _ticks++;
        }
        
        IsFinished = true;
    }

    private void Update() {
        // Go into world
        if (!world.TryGetActiveLevel(out INexitiesLevel? level)) return;
        
        // Get all logic systems required by active level
        
        // run all logic systems
        
        // cleanup
    }

    private void RunEndOfTick() {
        foreach (Action endOfUpdateAction in EndOfTickActions) endOfUpdateAction();
        EndOfTickActions.Clear();
    }

    private void SleepUntilEndOfTick() {
        TickStopwatch.Stop();
        double elapsed = TickStopwatch.Elapsed.TotalMilliseconds;
        
        // Logger.Debug("TickTime : {elapsed}ms ", elapsed);
        double sleepTime = MillisecondsPerTick - elapsed;
        if (sleepTime > 0) Thread.Sleep((int)sleepTime);
    }

    private void CalculateActualTps() {
        if (TpsStopwatch.ElapsedMilliseconds < 1000) return;
        
        ActualTps = _ticks;
        _ticks = 0;
        TpsStopwatch.Restart();
        
        eventManager.InvokeSendActualTps(ActualTps);
    }
    
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Event Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void RegisterEvents() {
        eventManager.EventStart += Start;
        eventManager.EventStop += Stop;
        eventManager.EventChangeActiveLevel += (_, args) => EndOfTickActions.Add(() => world.TryChangeActiveLevel(args.NewLevelId));
    }

    private void Start(object? _, EventArgs __) {
        IsStarted = true;
        Logger.Information("Thread started");
    }

    private void Stop(object? _, EventArgs __) {
        IsRunning = false;
        Logger.Information("Thread stopped");
    }
}
