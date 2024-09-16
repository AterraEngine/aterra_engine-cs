// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AbstractThreadProcessor : IThreadProcessor {
    public CancellationToken CancellationToken { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected static void RegisterTickEvents<TSystem>(ref TickEventHandler? eventHandler, TryGetSystemsDelegate<TSystem> func, Func<TSystem, TickEventHandler> callback) where TSystem : INexitiesSystem {
        if (!func(out TSystem[]? systems)) return;
        eventHandler = systems.Aggregate(eventHandler, (current, system) => current + callback(system));
    }
    protected static void RegisterClearCacheEvents<TSystem>(ref EmptyEventHandler? eventHandler, TryGetSystemsDelegate<TSystem> func, Func<TSystem, EmptyEventHandler> callback) where TSystem : INexitiesSystem {
        if (!func(out TSystem[]? systems)) return;
        eventHandler = systems.Aggregate(eventHandler, (current, system) => current + callback(system));
    }
    
    public abstract void RegisterEventsStartup();
    public abstract void Run();
    public abstract void OnLevelChangeStarted(IActiveLevel oldLevel);
    public abstract void OnLevelChangeCompleted(IActiveLevel newLevel);
}
