// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
    ITextureAtlas textureAtlas,
    ILogicEventManager eventManager
) : AbstractThread {
    
    public int TargetTicksPerSecond { get; set; } = 20; // TPS
    public bool IsStarted { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsFinished { get; private set; }
    
    public List<Action> EndOfTickActions { get; set; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Run Method
    // -----------------------------------------------------------------------------------------------------------------
    #region Run & Update
    public override void Run() {
        var stopwatch = Stopwatch.StartNew();

        // Wait until the main thread signaled to start
        while (!IsStarted) {
            stopwatch.Restart();
            SleepUntilEndOfTick(stopwatch);
        }

        // Game engine is actually running now
        while (IsRunning) {
            stopwatch.Restart();
            
            // Call UPDATE LOOP
            Update();
            RunEndOfTick();
            
            // Wait until the end of the Tick cycle
            SleepUntilEndOfTick(stopwatch);
        }
        
        IsFinished = true;
    }

    private void Update() {
        // Go into world
        
        // Get all logic systems required by active level
        
        // run all logic systems
        
        // cleanup
    }

    private void RunEndOfTick() {
        foreach (Action endOfUpdateAction in EndOfTickActions) {
            endOfUpdateAction();
        }
        EndOfTickActions.Clear();
    }

    private void SleepUntilEndOfTick(Stopwatch stopwatch) {
        stopwatch.Stop();
        double elapsed = stopwatch.Elapsed.TotalMilliseconds;
        logger.Debug("TickTime : {elapsed}ms ", elapsed);
            
        double sleepTime = elapsed / TargetTicksPerSecond;
        if (sleepTime > 0) Thread.Sleep((int)sleepTime);
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

    public void Start(object? _, EventArgs __) {
        IsRunning = true;
    }
    
    public void Stop(object? _, EventArgs __) {
        IsRunning = false;
    }
}
