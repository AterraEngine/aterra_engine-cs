// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.Threading2.CrossData;
using AterraCore.Contracts.Threading2.CrossData.Holders;
using JetBrains.Annotations;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Threading.CrossData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ICrossThreadDataAtlas>]
public class CrossThreadDataAtlas(IAssetInstanceAtlas instanceAtlas) : ICrossThreadDataAtlas {
    private readonly ConcurrentDictionary<AssetId, ICrossThreadData> _dataHolders = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void 
    
    public bool TryGetOrCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? dataHolder) where T : class, ICrossThreadData {
        dataHolder = null;
        if (!_dataHolders.TryGetValue(assetId, out ICrossThreadData? originalDataHolder)) {
            if (!instanceAtlas.TryGetOrCreate(assetId, out originalDataHolder)) return false;
            _dataHolders.TryAdd(assetId, originalDataHolder);
        }

        dataHolder = originalDataHolder as T;
        return dataHolder != null;
    }
    
    public bool TryGetOrCreateDataCollector([NotNullWhen(true)] out IDataCollector? dataHolder) =>
        TryGetOrCreate(AssetIdLib.AterraCore.CrossThreadDataHolders.DataCollector, out dataHolder);
    
    public bool TryGetOrCreateTextureBus([NotNullWhen(true)] out ITextureBus? dataHolder) =>
        TryGetOrCreate(AssetIdLib.AterraCore.CrossThreadDataHolders.TextureBus, out dataHolder);
}
