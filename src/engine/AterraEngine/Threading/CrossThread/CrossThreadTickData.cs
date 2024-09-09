// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
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
public class CrossThreadTickData : ICrossThreadTickData {
    private readonly ConcurrentDictionary<AssetTag, ITickDataHolder> _tickDataHolders = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryRegister<T>(AssetTag key, T tickDataHolder) where T : class, ITickDataHolder {
        if (_tickDataHolders.TryAdd(key, tickDataHolder)) return true;
        if (!_tickDataHolders.TryGetValue(key, out ITickDataHolder? value)) return false;
        var oldObject = value as T;
        return oldObject is not null && oldObject.IsEmpty;
    }

    public bool TryGet<T>(AssetTag key, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder {
        tickDataHolder = default;
        if (!_tickDataHolders.TryGetValue(key, out ITickDataHolder? value)) return false;
        tickDataHolder = value as T;
        return tickDataHolder != null;
    }

    public bool TryGetOrRegister<T>(AssetTag key, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder, new() {
        if (TryGet(key, out tickDataHolder)) return true;

        tickDataHolder = new T();
        if (TryRegister(key, tickDataHolder)) return true;

        tickDataHolder = null;
        return false;
    }

    public void Clear() {
        foreach (ITickDataHolder holder in _tickDataHolders.Values) {
            holder.Clear();
        }
    }
}
