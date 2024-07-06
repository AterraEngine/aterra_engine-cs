// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts;
using AterraCore.Contracts.Boot;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Helper Code
// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NewEngineConfiguration(ILogger? logger = null) : INewEngineConfiguration {
    private ILogger Logger { get; } = GetStartupLogger(logger);
    
    private NewConfigurationWarningAtlas ConfigurationWarningAtlas { get; } = new(GetStartupLogger(logger));
    
    private LinkedList<IBootOperation> OrderOfBootOperations { get; set; } = [];
    private Dictionary<AssetId, (IBootOperation Operation, AssetId? After, AssetId? Before)> Dependencies { get; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region StartupLogger Helper
    
    private static ILogger? _startupLogger;
    private static ILogger GetStartupLogger(ILogger? logger) => logger ?? ( _startupLogger ??= StartupLogger.CreateLogger(false).ForStartupContext());
   
    #endregion
    
    #region Register Boot Operations
    
    private bool TryResolveBootOperation(IBootOperation newOperation, AssetId? after, AssetId? before, LinkedList<IBootOperation>? nestedOperations = null) {
        LinkedList<IBootOperation> operations = nestedOperations ?? OrderOfBootOperations;
        if (operations.Find(n => n.AssetId == newOperation.AssetId) is not null) {
            Logger.Debug("No need to resolve {assetId}, as it was already present", newOperation.AssetId);
            return true;
        }
        
        Logger.Debug("Trying to resolve : {assetId}", newOperation.AssetId);
        
        switch (after, before) {
            // No dependencies were defined
            case (null, null) : {
                Logger.Debug("No dependencies were defined");
                operations.AddFirst(newOperation);
                return true; 
            }
            
            // Only a "before" dependency was defined
            //      AND the dependency is already present
            case (null, not null) when operations.Find(n => n.AssetId == (AssetId)before) is {} node: {
                Logger.Debug("Only a \"before\" dependency was defined AND the dependency is present");
                operations.AddBefore(node, newOperation);
                return true;
            }
            
            // Only a "before" dependency was defined
            //      BUT no operation under that AssetId was already present 
            case (null, not null) when operations.Find(n => n.AssetId == (AssetId)before) is null: {
                Logger.Debug("Only a \"before\" dependency was defined BUT operation under that AssetId was not present ");
                if (!TryResolveNested((AssetId)before, out LinkedListNode<IBootOperation>? node, operations)) return false;
                operations.AddBefore(node, newOperation);
                return true;
            }
            
            // Only an "after" dependency was defined
            //      AND the dependency is already present
            case (not null, null) when operations.Find(n => n.AssetId == (AssetId)after) is {} node: {
                Logger.Debug("Only an \"after\" dependency was defined AND the dependency is already present");
                operations.AddAfter(node, newOperation);
                return true;
            }
            
            // Only an "after" dependency was defined
            //      BUT no operation under that AssetId was already present 
            case (not null, null) when operations.Find(n => n.AssetId == (AssetId)after) is null: {
                Logger.Debug("Only an \"after\" dependency was defined BUT operation under that AssetId was not present ");
                if (!TryResolveNested((AssetId)after, out LinkedListNode<IBootOperation>? node, operations)) return false;
                operations.AddAfter(node, newOperation);
                return true;
            }
            
            // Both an "after" and a "before" dependencies were defined
            case (not null, not null): {
                Logger.Debug("Both dependencies were defined");
                // Try and find the "after" node
                LinkedListNode<IBootOperation>? afterNode = 
                    operations.Find(n => n.AssetId == (AssetId)after) 
                    ?? (TryResolveNested((AssetId)after, out LinkedListNode<IBootOperation>? aNode, operations) ? aNode : null);
                if (afterNode is null) return false;
                
                // Try and find the "before" node
                LinkedListNode<IBootOperation>? beforeNode = 
                    operations.Find(n => n.AssetId == (AssetId)before) 
                    ?? (TryResolveNested((AssetId)before, out LinkedListNode<IBootOperation>? bNode, operations) ? bNode : null);
                if (beforeNode is null) return false;
                
                if (!IsNodeBefore(afterNode, beforeNode)) return false;
                operations.AddBefore(beforeNode, newOperation);
                return true;
            } 
        }
        return false;
    }

    private bool VerifyDependencies(LinkedList<IBootOperation> operations) {
        LinkedListNode<IBootOperation>? node = operations.First;
        
        while (node is not null) {
            if (!Dependencies.TryGetValue(node.Value.AssetId, out (IBootOperation, AssetId?, AssetId?) tuple )) return false;
            (_, AssetId? after, AssetId? before) = tuple;

            if (after != null) {
                if (operations.Find(n => n.AssetId == after) is not {} afterNode 
                    || !IsNodeBefore(afterNode, node)
                ) return false;
            }

            if (before != null) {
                if (operations.Find(n => n.AssetId == before) is not {} beforeNode
                    || !IsNodeAfter(beforeNode, node)
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

    private static bool IsNodeAfter(LinkedListNode<IBootOperation> node, LinkedListNode<IBootOperation> referenceNode) {
        LinkedListNode<IBootOperation>? current = referenceNode;
        while (current != null) {
            if (current == node) return true;
            current = current.Next;
        }
        return false;
    }

    private bool TryResolveNested(AssetId assetId, [NotNullWhen(true)] out LinkedListNode<IBootOperation>? node, LinkedList<IBootOperation> nestedOperations) {
        node = default;
        if (!Dependencies.TryGetValue(assetId, out (IBootOperation, AssetId?, AssetId?) tuple)) return false;
        if (!TryResolveBootOperation(tuple.Item1, tuple.Item2, tuple.Item3, nestedOperations)) return false;
        if (nestedOperations.Find(n => n.AssetId == assetId) is not {} newNode) return false;
      
        node = newNode;
        return true;
    }
    
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public INewEngineConfiguration RegisterBootOperation(IBootOperation newOperation, AssetId? after = null, AssetId? before = null) {
        if (!Dependencies.TryAdd(newOperation.AssetId, (newOperation, after, before))) ConfigurationWarningAtlas.RaiseWarningEvent(UnstableBootOperationOrder, newOperation);
        return this;
    }
    
    public INewEngineConfiguration RunBootOperations() {
        Logger.Information("Started Resolving Boot Operations");
        foreach ((IBootOperation operation, AssetId? after, AssetId? before) in Dependencies.Values) {
            if (!TryResolveBootOperation(operation, after, before)) {
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

    public IEngine BuildEngine() {
        throw new NotImplementedException();
    }
}
