// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.Logic.PluginLoading;
using AterraCore.Contracts;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.DI;
using AterraCore.Loggers;
using CodeOfChaos.Extensions.Serilog;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineBuilder {
    private LinkedList<IBootOperation> OrderOfBootOperations { get; } = [];
    private ILogger Logger { get; } = GetStartupLogger();
    
    private BootComponents? _components;
    private BootComponents Components => _components ??= new BootComponents(
        PluginLoader: new FilePathPluginLoader(Logger)
    );

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    
    #region StartupLogger Helper
    private static ILogger? _startupLogger;
    private static ILogger GetStartupLogger() => _startupLogger ??= StartupLogger.CreateLogger(false).ForContext<EngineBuilder>();
    #endregion
    
    #region RegisterBootOperations
    public void RegisterBootOperations(Action<BootOperationConfiguration> action) {
        var config = new BootOperationConfiguration(OrderOfBootOperations);
        action(config);
    }
    
    public class BootOperationConfiguration(LinkedList<IBootOperation> operations) {
        public void AddOperation<T>() where T : IBootOperation => AddOperation(Activator.CreateInstance<T>());
        public void AddOperation<T>(T operation ) where T : IBootOperation => operations.AddLast(operation);
    }
    #endregion
    
    #region BuildEngine
    public IEngine BuildEngine() {
        // Log operation order
        LogOrderOfBootOperations();

        //  Run all the boot operations
        foreach (IBootOperation operation in OrderOfBootOperations) {
            operation.Run(Components);
        }

        // Create the Actual Engine
        //  Should be the last step
        IEngine engine = EngineServices.GetEngine();
        Logger.Information("Engine instance created of type: {Type}", engine.GetType().FullName);
        return engine;
    }
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
}
