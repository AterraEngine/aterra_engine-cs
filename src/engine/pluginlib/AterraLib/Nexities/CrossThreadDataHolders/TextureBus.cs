// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Contracts.Threading2.CrossData.Holders;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.CrossThreadDataHolders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CrossThreadDataHolder<ITextureBus>(StringAssetIdLib.AterraCore.CrossThreadDataHolders.TextureBus)]
public class TextureBus : AssetInstance, ITextureBus {
    public ConcurrentQueue<AssetId> TexturesToLoad { get; } = [];
    public ConcurrentQueue<AssetId> TexturesToUnLoad { get; } = [];
    
    public bool IsEmpty => TexturesToLoad.IsEmpty && TexturesToUnLoad.IsEmpty;
}
