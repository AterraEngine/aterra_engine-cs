// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading;
using AterraCore.Contracts;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.DI;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;

namespace AterraCore.Boot;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfiguration(ILogger? logger = null) : IEngineConfiguration {
    private ILogger Logger { get; } = GetStartupLogger(logger);
    
    private WarningAtlas ConfigurationWarningAtlas { get; } = new(GetStartupLogger(logger));
    
    private LinkedList<IBootOperation> OrderOfBootOperations { get; } = [];

    private BootOperationComponents? _components;
    private BootOperationComponents Components => _components ??= new BootOperationComponents(
        WarningAtlas:ConfigurationWarningAtlas,
        PluginLoader:new FilePathPluginLoader(Logger)
    );
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region StartupLogger Helper
    private static ILogger? _startupLogger;
    private static ILogger GetStartupLogger(ILogger? logger) => 
        logger ?? ( _startupLogger ??= StartupLogger.CreateLogger(false).ForContext<EngineConfiguration>());
    #endregion

    #region LogOrderOfBootOperations
    private void LogOrderOfBootOperations() {
        var builder = new ValuedStringBuilder();
        builder.AppendLine("Order of Boot Operations:");
       
        foreach (IBootOperation operation in OrderOfBootOperations) {
            builder.AppendLineValued("- ", operation.GetType().FullName);
        }
        Logger.Information(builder); 
    }
    #endregion
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region RegisterBootOperations
    public IEngineConfiguration RegisterBootOperation<T>() where T : IBootOperation, new() => RegisterBootOperation(new T());
    public IEngineConfiguration RegisterBootOperation(IBootOperation newOperation) {
        OrderOfBootOperations.AddLast(newOperation);
        return this;
    }
    #endregion
    
    #region BuildEngine
    public IEngine BuildEngine() {
        // Log operation order
        LogOrderOfBootOperations();
        
        //  Run all the boot operations
        OrderOfBootOperations.IterateOver(
            operation => operation.Run(Components)
        );
        
        // Populate Plugin Atlas with plugin list
        //      Is a singleton anyway, so doesn't matter when we assign this data
        Logger.Information("Preloading the Engine {i} plugins", Components.ValidPlugins.Length);
        EngineServices.GetPluginAtlas().ImportLoadedPluginDtos(Components.ValidPlugins);

        // Create the Actual Engine
        //  Should be the last step
        IEngine engine = EngineServices.GetEngine();
        Logger.Information("Engine instance created of type: {Type}", engine.GetType().FullName);
        return engine;
    }
    #endregion
}
