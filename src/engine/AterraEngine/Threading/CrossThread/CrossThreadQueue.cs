// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.Contracts.Threading.CrossThread.Dto;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AterraEngine.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ICrossThreadQueue>]
public class CrossThreadQueue(ILogger logger) : ICrossThreadQueue {
    private ILogger Logger { get; } = logger.ForContext<CrossThreadQueue>();

    public ConcurrentQueue<TextureRegistrar> TextureRegistrarQueue { get; } = new();

    private ConcurrentDictionary<QueueKey, ConcurrentQueue<Action>> GeneralActionQueue { get; } = new();

    public bool EntireQueueIsEmpty => GeneralActionQueue.IsEmpty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryDequeue(QueueKey key, [NotNullWhen(true)] out Action? action) {
        action = default;
        return GeneralActionQueue.TryGetValue(key, out ConcurrentQueue<Action>? queue)
            && queue.TryDequeue(out action);
    }

    public bool TryEnqueue(QueueKey key, Action action) {
        try {
            ConcurrentQueue<Action> queue = GeneralActionQueue.GetOrAdd(key, valueFactory: _ => new ConcurrentQueue<Action>());
            queue.Enqueue(action);
            return true;
        }
        catch (Exception ex) {
            Logger.Warning(ex, "failed to enqueue at {key}", key);
            return false;
        }
    }
}
