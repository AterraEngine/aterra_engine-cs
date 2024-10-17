// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading.CrossData.Holders;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Threading.CrossData;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadDataAtlas {
    ITextureBus TextureBus { get; }
    IDataCollector DataCollector { get; }
    ILevelChangeBus LevelChangeBus { get; }

    bool ResetActiveLevel { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryGet<T>(AssetId assetId, [NotNullWhen(true)] out T? dataHolder) where T : class, ICrossThreadData;
    bool TryGetOrCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? dataHolder) where T : class, ICrossThreadData;

    void CleanupRenderTick();
    void CleanupLogicTick();
}
