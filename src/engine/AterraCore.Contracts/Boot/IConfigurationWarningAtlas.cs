// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IWarningAtlas {
    public IWarning GetWarning(AssetId assetId);
    public bool TryRegisterWarning(AssetId assetId, IWarning warning);
    public void AddEvent(AssetId assetId, EventHandler<WarningEventArgs> eventHandler);
    public void RaiseEvent(AssetId assetId, object? sender = null, params object?[] messageParams);
}