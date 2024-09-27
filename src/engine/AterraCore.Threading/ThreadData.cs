﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Threading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct ThreadData(CancellationTokenSource cts, Thread thread) {
    public CancellationTokenSource CancellationTokenSource { get; } = cts;
    public Thread Thread { get; } = thread;
}
