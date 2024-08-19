// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct ThreadData(CancellationTokenSource cts, Thread thread) : IThreadData {
    public CancellationTokenSource CancellationTokenSource { get; } = cts;
    public Thread Thread { get; } = thread;
}
