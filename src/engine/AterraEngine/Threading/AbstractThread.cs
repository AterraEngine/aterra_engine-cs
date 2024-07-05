﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Threading;
using System.Collections.Concurrent;

namespace AterraEngine.Threading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public abstract class AbstractThread : IThread {
    public CancellationToken CancellationToken { get; internal set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Run();
}
