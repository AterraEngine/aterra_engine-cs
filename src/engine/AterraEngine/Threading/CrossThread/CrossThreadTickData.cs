// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.Threading.CrossThread;
using JetBrains.Annotations;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ICrossThreadTickData>]
public class CrossThreadTickData(ICrossThreadEventManager crossThreadEventManager, IAssetInstanceAtlas instanceAtlas) : ICrossThreadTickData {
    private readonly ConcurrentDictionary<AssetId, ITickDataHolder> _tickDataHolders = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // ReSharper disable once SuspiciousTypeConversion.Global
    // ReSharper disable once ConvertIfStatementToSwitchStatement
    public bool TryRegister<T>(AssetId assetId) where T : class, ITickDataHolder {
        if (!instanceAtlas.TryGetOrCreate(assetId, out T? tickDataHolder)) return false;
        if (!_tickDataHolders.TryAdd(assetId, tickDataHolder)) return false;

        // attack to events
        if (tickDataHolder is IHasLevelChangeCleanup levelChangeCleanup) crossThreadEventManager.LevelChangeCleanup += levelChangeCleanup.OnLevelChangeCleanup;
        if (tickDataHolder is IHasLogicTickCleanup logicCleanup) crossThreadEventManager.LogicTickCleanup += logicCleanup.OnLogicTickCleanup;
        if (tickDataHolder is IHasRenderTickCleanup renderCleanup) crossThreadEventManager.RenderTickCleanup += renderCleanup.OnRenderTickCleanup;
        
        return true;
    }

    public bool TryGet<T>(AssetId assetId, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder {
        tickDataHolder = default;
        if (!_tickDataHolders.TryGetValue(assetId, out ITickDataHolder? value)) return false;

        tickDataHolder = value as T;
        return tickDataHolder != null;
    }

    public bool TryGetOrRegister<T>(AssetId assetId, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder {
        if (TryGet(assetId, out tickDataHolder)) return true;
        if (!TryRegister<T>(assetId)) return false;
        if (TryGet(assetId, out tickDataHolder)) return true;
        tickDataHolder = null;
        return false;
    }
    
    public bool TryGetNonEmpty<T>(AssetId assetId, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder => TryGet(assetId, out tickDataHolder) && !tickDataHolder.IsEmpty;
    public bool TryGetOrRegisterNonEmpty<T>(AssetId assetId, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder => TryGetOrRegister(assetId, out tickDataHolder) && !tickDataHolder.IsEmpty;
}
