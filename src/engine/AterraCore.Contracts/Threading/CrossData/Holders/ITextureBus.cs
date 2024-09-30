// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Collections.Concurrent;

namespace AterraCore.Contracts.Threading.CrossData.Holders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITextureBus : ICrossThreadData {
    ConcurrentQueue<AssetId> TexturesToLoad { get; }
    ConcurrentQueue<AssetId> TexturesToUnLoad { get; }

    bool IsEmpty { get; }
}
