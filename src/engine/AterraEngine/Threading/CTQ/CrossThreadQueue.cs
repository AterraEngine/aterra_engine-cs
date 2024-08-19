// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CTQ;
using AterraCore.Contracts.Threading.CTQ.Dto;
using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace AterraEngine.Threading.CTQ;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CrossThreadQueue : ICrossThreadQueue {
    public ConcurrentQueue<TextureRegistrar> TextureRegistrarQueue { get; } = new();
}
