// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NewConfigurationWarningAtlas(ILogger logger) : IConfigurationWarningAtlas {
    private ILogger Logger { get; } = logger.ForConfigurationWarningAtlasContext();
    
    private static readonly IWarning UndefinedWarning = new NewWarning(nameof(Undefined));
    private readonly Dictionary<AssetId, IWarning> _warnings = new() {
        { Undefined, UndefinedWarning }, 
        { UnstableFlexiPlugLoadOrder, new NewWarning(nameof(UnstableFlexiPlugLoadOrder)) },
        { UnstableBootOperationOrder, new NewWarning(nameof(UnstableBootOperationOrder)) },
        { UnableToLoadEngineConfigFile, new NewWarning(nameof(UnableToLoadEngineConfigFile)) },
        { EngineOverwritten, new NewWarning(nameof(EngineOverwritten)) },
    };
    
    private readonly Dictionary<AssetId, EventHandler<WarningEventArgs>> _eventHandlers = new() {
        {UnstableBootOperationOrder, (sender, args) => logger.ExitFatal(
            (int)ExitCodes.UnstableBootOperationOrder,
            args.Warning.MessageTemplate ?? "Something went wrong during configuration operation building. Values of {name} were : {@sender}",
            sender?.GetType(), sender
        )}
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IWarning GetWarning(AssetId assetId) => _warnings.GetValueOrDefault(assetId, UndefinedWarning);
    public bool TryRegisterWarning(AssetId assetId, IWarning warning) {
        if (!_warnings.TryAdd(assetId, warning)) return false;
        Logger.Debug("Registered {assetId} as a warning during engine configuration", assetId);
        return true;
    }

    public void AddWarningEvent(AssetId assetId, EventHandler<WarningEventArgs> eventHandler) {
        _eventHandlers.AddOrUpdate(assetId, eventHandler);
    }
    
    public void RaiseWarningEvent(AssetId assetId, object? sender = null, params object?[] messageParams) {
        if (!_eventHandlers.TryGetValue(assetId, out EventHandler<WarningEventArgs>? eventHandler)) return;
        eventHandler.Invoke(sender, new WarningEventArgs(GetWarning(assetId), messageParams));
    }
}
