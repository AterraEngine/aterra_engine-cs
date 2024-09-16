// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Collections.Concurrent;

namespace AterraCore.Contracts.Threading.CrossThread.TickDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITextureDataHolder : ITickDataHolder {
    ConcurrentQueue<AssetId> TexturesToLoad { get; }
    ConcurrentQueue<AssetId> TexturesToUnLoad { get; }
}
