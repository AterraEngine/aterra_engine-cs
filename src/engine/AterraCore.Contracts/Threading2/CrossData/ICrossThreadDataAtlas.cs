// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading2.CrossData.Holders;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Threading2.CrossData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadDataAtlas {
    bool TryGetOrCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? dataHolder) where T : class, ICrossThreadData;
    
    bool TryGetOrCreateDataCollector([NotNullWhen(true)] out IDataCollector? dataHolder);
    bool TryGetOrCreateTextureBus([NotNullWhen(true)] out ITextureBus? dataHolder);
}
