// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading;
using AterraCore.DI;
using AterraEngine.Threading.Logic;
using AterraEngine.Threading.Render;
using JetBrains.Annotations;
using Serilog;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ThreadingManager(ILogger logger) : IThreadingManager {
    private ILogger Logger { get; } = logger.ForContext<ThreadingManager>();
    
    public IThreadData? LogicThreadData { get; private set; }
    public IThreadData? RenderThreadData { get; private set; } 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<bool> TrySpawnRenderThreadAsync() {
        var cts = new CancellationTokenSource();
        
        try {
            var threadProcessor = EngineServices.CreateWithServices<RenderThreadProcessor>();
            threadProcessor.CancellationToken = cts.Token;
            
            // Add extra callbacks
            threadProcessor.RegisterEvents();
            
            // Actually spawn the thread
            var renderThread = new Thread(threadProcessor.Run);
            renderThread.Start();
            RenderThreadData = new ThreadData(cts, renderThread);

            return true;
        }
        catch (Exception ex) {
            Logger.Error(ex, "An error occured while spawning render thread");
            await cts.CancelAsync();
            return false;
        }
    }

    public async Task<bool> TrySpawnLogicThreadAsync() {
        var cts = new CancellationTokenSource();
        
        try {
            var threadProcessor = EngineServices.CreateWithServices<LogicThreadProcessor>();
            threadProcessor.CancellationToken = cts.Token;
            
            // Add extra callbacks
            threadProcessor.RegisterEvents();
            
            // Actually spawn the thread
            var renderThread = new Thread(threadProcessor.Run);
            renderThread.Start();
            LogicThreadData = new ThreadData(cts, renderThread);

            return true;
        }
        catch (Exception ex) {
            Logger.Error(ex, "An error occured while spawning logic thread");
            await cts.CancelAsync();
            return false;
        }
    }

}