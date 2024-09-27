// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Threading2.Cross.Queue;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadQueue {
    void EnqueueToRenderThread(AssetTag assetTag, IQueueHolder? queueHolder = null) ;
    bool TryDequeueFromRenderThread(out AssetTag assetTag, [NotNullWhen(true)] out IQueueHolder? queueHolder) ;
    
    void EnqueueToLogicThread(AssetTag assetTag, IQueueHolder? queueHolder = null) ;
    bool TryDequeueFromLogicThread(out AssetTag assetTag, [NotNullWhen(true)] out IQueueHolder? queueHolder) ;
}
