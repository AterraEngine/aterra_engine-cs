// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICrossThreadTickData {
    bool TryRegister<T>(AssetTag key, T tickDataHolder) where T : class, ITickDataHolder;
    bool TryGet<T>(AssetTag key, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder;
    bool TryGetOrRegister<T>(AssetTag key, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder, new();

    void ClearOnLevelChange();
    void ClearOnLogicTick();
    void ClearOnRenderFrame();
}
