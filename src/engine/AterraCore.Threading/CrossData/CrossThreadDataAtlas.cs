// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.Threading.CrossData;
using AterraCore.Contracts.Threading.CrossData.Holders;
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

    private ITextureBus? _textureBus;
    public ITextureBus TextureBus => _textureBus ??= TryGetOrCreate(AssetIdLib.AterraCore.CrossThreadDataHolders.TextureBus, out ITextureBus? dataHolder) ? dataHolder : throw new Exception();

    private IDataCollector? _dataCollector;
    public IDataCollector DataCollector => _dataCollector ??= TryGetOrCreate(AssetIdLib.AterraCore.CrossThreadDataHolders.DataCollector, out IDataCollector? dataHolder) ? dataHolder : throw new Exception();

    private ILevelChangeBus? _levelChangeBus;
    public ILevelChangeBus LevelChangeBus => _levelChangeBus ??= TryGetOrCreate(AssetIdLib.AterraCore.CrossThreadDataHolders.LevelChangeBus, out ILevelChangeBus? dataHolder) ? dataHolder : throw new Exception();
    
    public bool ResetActiveLevel { get; set; } = false;

    private event RenderTickCleanupDelegate? RenderTickCleanups;
    private event RenderTickCleanupDelegate? LogicTickCleanups;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGet<T>(AssetId assetId, [NotNullWhen(true)] out T? dataHolder) where T : class, ICrossThreadData {
        dataHolder = null;
        if (!_dataHolders.TryGetValue(assetId, out ICrossThreadData? originalDataHolder)) return false;

        dataHolder = originalDataHolder as T;
        return dataHolder != null;
    }

    public bool TryGetOrCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? dataHolder) where T : class, ICrossThreadData {
        dataHolder = null;
        if (!_dataHolders.TryGetValue(assetId, out ICrossThreadData? originalDataHolder)) {
            if (!instanceAtlas.TryGetOrCreate(assetId, out originalDataHolder)) return false;

            _dataHolders.TryAdd(assetId, originalDataHolder);

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (originalDataHolder is IHasRenderTickCleanup hasRenderTickCleanup) RenderTickCleanups += hasRenderTickCleanup.OnRenderTickCleanup;
            if (originalDataHolder is IHasLogicTickCleanup hasLogicTickCleanup) LogicTickCleanups += hasLogicTickCleanup.OnLogicTickCleanup;
        }

        dataHolder = originalDataHolder as T;
        return dataHolder != null;
    }

    public void CleanupRenderTick() => RenderTickCleanups?.Invoke();
    public void CleanupLogicTick() => LogicTickCleanups?.Invoke();
}
