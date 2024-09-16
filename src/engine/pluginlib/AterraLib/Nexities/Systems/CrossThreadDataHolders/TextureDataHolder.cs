// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Threading.CrossThread.TickDataHolders;
using AterraCore.OmniVault.Assets;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.Systems.CrossThreadDataHolders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[DataHolder(StringAssetIdLib.AterraLib.TickDataHolders.TextureData)]
public class TextureDataHolder : AssetInstance, ITextureDataHolder {
    public ConcurrentQueue<AssetId> TexturesToLoad { get; } = new();
    public ConcurrentQueue<AssetId> TexturesToUnLoad { get; } = new();
    
    public bool IsEmpty => TexturesToLoad.IsEmpty && TexturesToUnLoad.IsEmpty;
}
