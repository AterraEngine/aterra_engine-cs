// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Threading.CrossThread;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CrossThreadQueue : ICrossThreadQueue {
    public ConcurrentQueue<TextureRegistrar> TextureRegistrarQueue { get; } = new();

    private ConcurrentDictionary<QueueKey, ConcurrentQueue<Action>> ActionQueue { get; } = new();
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryDequeue(QueueKey key, [NotNullWhen(true)] out Action? action) {
        action = null;
        return ActionQueue.TryGetValue(key, out ConcurrentQueue<Action>? queue)
               && queue.TryDequeue(out action);
    }
    
    public bool TryEnqueue(QueueKey key, Action action) {
        ConcurrentQueue<Action> queue = ActionQueue.GetOrAdd(key, _ => new ConcurrentQueue<Action>());
        queue.Enqueue(action);
        return true;
    }
}

