// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading;
using AterraCore.DI;
using Serilog;

namespace AterraCore.Threading.Threads;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AbstractThreadProcessor<T> : IThreadProcessor {
    protected Stack<Action> EndOfTickActions { get; } = [];
    protected ILogger Logger { get; } = EngineServices.GetLogger().ForContext<T>();
    
    public CancellationToken CancellationToken { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods 
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Run();
    public void AddToEndOfTick(Action action) => EndOfTickActions.Push(action);
}
