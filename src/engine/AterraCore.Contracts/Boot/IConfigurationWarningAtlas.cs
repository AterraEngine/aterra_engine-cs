// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IConfigurationWarningAtlas {
    public IWarning GetWarning(AssetId assetId);
    public bool TryRegisterWarning(AssetId assetId, IWarning warning);
    public void AddWarningEvent(AssetId assetId, EventHandler<WarningEventArgs> eventHandler);
    public void RaiseWarningEvent(AssetId assetId, object? sender = null, params object?[] messageParams);
}