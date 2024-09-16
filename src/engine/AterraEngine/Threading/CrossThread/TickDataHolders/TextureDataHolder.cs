// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading.CrossThread.TickDataHolders;
using System.Collections.Concurrent;

namespace AterraEngine.Threading.CrossThread.TickDataHolders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextureDataHolder : ITextureDataHolder {
    public ConcurrentQueue<AssetId> TexturesToLoad { get; } = new();
    public ConcurrentQueue<AssetId> TexturesToUnLoad { get; } = new();
    
    public bool IsEmpty => TexturesToLoad.IsEmpty && TexturesToUnLoad.IsEmpty;
}
