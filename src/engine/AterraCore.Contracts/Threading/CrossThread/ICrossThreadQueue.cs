// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CrossThread.Dto;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadQueue {
    ConcurrentQueue<TextureRegistrar> TextureRegistrarQueue { get; }

    bool EntireQueueIsEmpty { get; }

    bool TryDequeue(QueueKey key, [NotNullWhen(true)] out Action? action);
    bool TryEnqueue(QueueKey key, Action action);
}
