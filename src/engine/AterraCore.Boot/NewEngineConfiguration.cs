// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using System.Diagnostics.CodeAnalysis;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NewEngineConfiguration(ILogger? logger = null) : INewEngineConfiguration {
    private ILogger Logger { get; } = GetStartupLogger(logger);
    
    private NewConfigurationWarningAtlas ConfigurationWarningAtlas { get; } = new(GetStartupLogger(logger));
    
    private LinkedList<IBootOperation> OrderOfBootOperations { get; } = [];
    private Dictionary<AssetId, (IBootOperation Operation, AssetId? After)> Dependencies { get; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region StartupLogger Helper
    private static ILogger? _startupLogger;
    private static ILogger GetStartupLogger(ILogger? logger) => logger ?? ( _startupLogger ??= StartupLogger.CreateLogger(false).ForStartupContext());
    #endregion
    
    #region Register Boot Operations
    private bool TryResolveBootOperation(IBootOperation newOperation, AssetId? after, LinkedList<IBootOperation>? nestedOperations = null, int depthIndentations = 0) {
        LinkedList<IBootOperation> operations = nestedOperations ?? OrderOfBootOperations;
        string depth = new(' ', 4*depthIndentations);
        
        if (operations.Find(n => n.AssetId == newOperation.AssetId) is not null) {
            Logger.Debug("{depth}No need to resolve {assetId}, as it was already present", depth,newOperation.AssetId);
            return true;
        }
        
        Logger.Debug("{depth}Trying to resolve : {assetId}", depth, newOperation.AssetId );
        
        switch (after) {
            // No dependencies were defined
            case null : {
                Logger.Debug("{depth}No dependencies were defined", depth);
                operations.AddFirst(newOperation);
                return true; 
            }
            
            // Only an "after" dependency was defined
            //      AND the dependency is already present
            case not null when operations.Find(n => n.AssetId == (AssetId)after) is {} node: {
                Logger.Debug("{depth}Only an \"after\" dependency was defined AND the dependency is already present", depth);
                operations.AddAfter(node, newOperation);
                return true;
            }
            
            // Only an "after" dependency was defined
            //      BUT no operation under that AssetId was already present 
            case not null when operations.Find(n => n.AssetId == (AssetId)after) is null: {
                Logger.Debug("{depth}Only an \"after\" dependency was defined BUT operation under that AssetId was not present ", depth);
                if (!TryResolveNested((AssetId)after, out LinkedListNode<IBootOperation>? node, operations, depthIndentations)) return false;
                operations.AddAfter(node, newOperation);
                return true;
            }
        }
        return false;
    }

    private bool VerifyDependencies(LinkedList<IBootOperation> operations) {
        LinkedListNode<IBootOperation>? node = operations.First;
        
        while (node is not null) {
            if (!Dependencies.TryGetValue(node.Value.AssetId, out (IBootOperation, AssetId?) tuple )) return false;
            (_, AssetId? after) = tuple;

            if (after != null) {
                if (operations.Find(n => n.AssetId == after) is not {} afterNode 
                    || !IsNodeBefore(afterNode, node)
                ) return false;
            }
            
            node = node.Next;
        }

        Logger.Debug("Dependencies Verified");
        return true;
    }

    private static bool IsNodeBefore(LinkedListNode<IBootOperation> node, LinkedListNode<IBootOperation> referenceNode) {
        LinkedListNode<IBootOperation>? current = node;
        while (current != null) {
            if (current == referenceNode) return true;
            current = current.Next;
        }
        return false;
    }
    
    private bool TryResolveNested(AssetId assetId, [NotNullWhen(true)] out LinkedListNode<IBootOperation>? node, LinkedList<IBootOperation> nestedOperations, int depthIndentations = 0) {
        node = default;
        if (!Dependencies.TryGetValue(assetId, out (IBootOperation, AssetId?) tuple)) return false;
        if (!TryResolveBootOperation(tuple.Item1, tuple.Item2, nestedOperations, ++depthIndentations)) return false;
        if (nestedOperations.Find(n => n.AssetId == assetId) is not {} newNode) return false;
      
        node = newNode;
        return true;
    }
    
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region RegisterBootOperations
    public INewEngineConfiguration RegisterBootOperation<T>() where T : IBootOperation, new() => RegisterBootOperation(new T());
    public INewEngineConfiguration RegisterBootOperation(IBootOperation newOperation) {
        if (!Dependencies.TryAdd(newOperation.AssetId, (newOperation, newOperation.RanAfter))) ConfigurationWarningAtlas.RaiseWarningEvent(UnstableBootOperationOrder, newOperation);
        return this;
    }
    #endregion
    
    #region RunBootOperation
    public INewEngineConfiguration RunBootOperations() {
        Logger.Information("Started Resolving Boot Operations");
        foreach ((IBootOperation operation, AssetId? after) in Dependencies.Values) {
            if (!TryResolveBootOperation(operation, after)) {
                Logger.Warning("Operation {operation} could not resolved", operation );
            } else {
                Logger.Information("Operation {operation} resolved correctly", operation );
            }
        }
        if (!VerifyDependencies(OrderOfBootOperations)) Logger.ThrowFatal<SystemException>("Operations were not able to be verified");
        
        // Actually stuff
        var components = new BootOperationComponents(
        WarningAtlas: ConfigurationWarningAtlas
        );
        
        foreach (IBootOperation operation in OrderOfBootOperations) {
            operation.Run(components);
        }
        
        return this;
    }
    #endregion

    #region BuildEngine
    public IEngine BuildEngine() {
        throw new NotImplementedException();
    }
    #endregion
}
