// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
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
public class CrossThreadTickData(ICrossThreadEventManager crossThreadEventManager) : ICrossThreadTickData {
    private readonly ConcurrentDictionary<AssetTag, ITickDataHolder> _tickDataHolders = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryRegister<T>(AssetTag key) where T : class, ITickDataHolder, new() => TryRegister(key, new T());
    public bool TryRegister<T>(AssetTag key, out T dataHolder) where T : class, ITickDataHolder, new() {
        dataHolder = new T();
        return TryRegister(key, dataHolder);
    }

    // ReSharper disable once SuspiciousTypeConversion.Global
    // ReSharper disable once ConvertIfStatementToSwitchStatement
    public bool TryRegister<T>(AssetTag key, T tickDataHolder) where T : class, ITickDataHolder {
        if (!_tickDataHolders.TryAdd(key, tickDataHolder)) return false;

        // attack to events
        if (tickDataHolder is IHasLevelChangeCleanup levelChangeCleanup) crossThreadEventManager.LevelChangeCleanup += levelChangeCleanup.OnLevelChangeCleanup;
        if (tickDataHolder is IHasLogicTickCleanup logicCleanup) crossThreadEventManager.LogicTickCleanup += logicCleanup.OnLogicTickCleanup;
        if (tickDataHolder is IHasRenderTickCleanup renderCleanup) crossThreadEventManager.RenderTickCleanup += renderCleanup.OnRenderTickCleanup;
        
        return true;
    }

    public bool TryGet<T>(AssetTag key, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder {
        tickDataHolder = default;
        if (!_tickDataHolders.TryGetValue(key, out ITickDataHolder? value)) return false;

        tickDataHolder = value as T;
        return tickDataHolder != null;
    }

    public bool TryGetOrRegister<T>(AssetTag key, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder, new() {
        if (TryGet(key, out tickDataHolder)) return true;
        if (TryRegister<T>(key, out tickDataHolder)) return true;
        tickDataHolder = null;
        return false;
    }
}
