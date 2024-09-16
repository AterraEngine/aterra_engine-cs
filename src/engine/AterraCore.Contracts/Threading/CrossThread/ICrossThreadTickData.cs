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
    bool TryRegister<T>(AssetId assetId) where T : class, ITickDataHolder;
    bool TryGet<T>(AssetId assetId, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder;
    bool TryGetOrRegister<T>(AssetId assetId, [NotNullWhen(true)] out T? tickDataHolder) where T : class, ITickDataHolder;
}
