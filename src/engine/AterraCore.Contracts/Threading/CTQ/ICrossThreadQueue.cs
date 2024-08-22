// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CTQ.Dto;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Threading.CTQ;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadQueue {
    ConcurrentQueue<TextureRegistrar> TextureRegistrarQueue { get; }
    bool EntireQueueIsEmpty { get; }
    
    bool TryDequeue(QueueKey key, [NotNullWhen(true)] out Action? action);
    bool TryEnqueue(QueueKey key, Action action);
}
