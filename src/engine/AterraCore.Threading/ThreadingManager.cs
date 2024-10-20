﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Contracts.Threading;
using AterraCore.Contracts.Threading.Logic;
using AterraCore.Contracts.Threading.Rendering;
using AterraCore.DI;
using JetBrains.Annotations;
using Serilog;

namespace AterraCore.Threading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IThreadingManager>]
public class ThreadingManager(ILogger logger) : IThreadingManager {

    private readonly List<CancellationToken> _cancellationTokens = [];
    private ILogger Logger { get; } = logger.ForContext<ThreadingManager>();

    private ThreadData? LogicThreadData { get; set; }
    private ThreadData? RenderThreadData { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<bool> TrySpawnLogicThreadAsync() {
        var cts = new CancellationTokenSource();

        try {
            var threadProcessor = EngineServices.GetService<ILogicThreadProcessor>();
            threadProcessor.CancellationToken = cts.Token;
            _cancellationTokens.Add(threadProcessor.CancellationToken);

            // Actually spawn the thread
            var threadData = new ThreadData(
                cts,
                new Thread(threadProcessor.Run) {
                    Priority = ThreadPriority.Highest,
                    Name = "ThreadLogic"
                }
            );

            LogicThreadData = threadData;
            threadData.Thread.Start();

            return true;
        }
        catch (Exception ex) {
            Logger.Error(ex, "An error occured while spawning logic thread");
            await cts.CancelAsync();
            return false;
        }
    }

    public async Task<bool> TrySpawnRenderThreadAsync() {
        var cts = new CancellationTokenSource();

        try {
            var threadProcessor = EngineServices.GetService<IRenderThreadProcessor>();
            threadProcessor.CancellationToken = cts.Token;
            _cancellationTokens.Add(threadProcessor.CancellationToken);

            // Actually spawn the thread
            var threadData = new ThreadData(
                cts,
                new Thread(threadProcessor.Run) {
                    Priority = ThreadPriority.Highest,
                    Name = "ThreadRender"
                }
            );

            RenderThreadData = threadData;
            threadData.Thread.Start();

            return true;
        }
        catch (Exception ex) {
            Logger.Error(ex, "An error occured while spawning render thread");
            await cts.CancelAsync();
            return false;
        }
    }

    public void CancelThreads() {
        LogicThreadData?.CancellationTokenSource.Cancel();
        RenderThreadData?.CancellationTokenSource.Cancel();
    }

    public WaitHandle[] GetWaitHandles() {
        return _cancellationTokens
            .Select(cancellationToken => cancellationToken.WaitHandle)
            .ToArray();
    }

    public void JoinThreads() {
        LogicThreadData?.Thread.Join();
        RenderThreadData?.Thread.Join();
    }
}
