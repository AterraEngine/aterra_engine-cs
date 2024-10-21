// Imports
using AterraEngine.Contracts.Builder.BootOperations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraEngine.Builder.BootOperations;

// Code
public class BootOperationBuilder(BootOperationConfig config) : IBootOperationBuilder {
    public async Task BuildAsync(IServiceCollection serviceCollection, CancellationToken cancellationToken = default) {
        // Assemble chains
        List<Task<IChainVariables>> chains = AssembleChains(cancellationToken);

        // Execute the chains
        await Task.WhenAll(chains);
        if (chains.Any(task => task.IsFaulted || task.IsCanceled)) {
            throw new OperationCanceledException();
        }

        // Collect data from the chain results
        Dictionary<string, IChainVariables> results = chains
            .Select(task => task.Result)
            .ToDictionary(var => var.ChainName, var => var);

        IEnumerable<IChainVariables> ordered = config.ChainJoins.Select(
            chainName => results[chainName]
        );

        foreach (IChainVariables variables in ordered) {
            foreach (ServiceDescriptor service in variables.Services) {
                serviceCollection.Add(service);
            }
        }
    }

    private List<Task<IChainVariables>> AssembleChains(CancellationToken cancellationToken = default) {
        var tasks = new List<Task<IChainVariables>>();

        foreach ((string chainName, IBootOperationChain chain) in config.Chains) {
            Task<IChainVariables>? rootTask = null;
            Task<IChainVariables>? currentTask = null;

            foreach (Type operationType in chain) {
                object operationInstance = CreateOperationInstance(operationType, chain);

                if (rootTask == null) {
                    IChainVariables variables = CreateChainVariables(chain, chainName, operationType);
                    rootTask = ExecuteAsync(operationType, operationInstance, variables, cancellationToken);
                    currentTask = rootTask;
                    continue;
                }

                // Chain the operations
                currentTask = currentTask!
                    .ContinueWith(task => ExecuteAsync(operationType, operationInstance, task.Result, cancellationToken), cancellationToken)
                    .Unwrap();
            }

            if (rootTask is null) {
                throw new InvalidOperationException($"Failed to create boot operation chain '{chain.GetType().Name}'.");
            }
            tasks.Add(rootTask);
        }

        return tasks;
    }

    private static IChainVariables CreateChainVariables(IBootOperationChain chain, string chainName, Type operationType) {
        if (Activator.CreateInstance(chain.ChainVariableType, chainName) is not IChainVariables chainVariables) {
            throw new InvalidOperationException($"Failed to create chain variables for '{operationType.Name}'");
        }
        return chainVariables;
    }

    private static object CreateOperationInstance(Type operationType, IBootOperationChain chain) {
        if (Activator.CreateInstance(operationType) is not { } operationInstance) {
            throw new InvalidOperationException($"Failed to create boot operation '{operationType.Name}'.");
        }

        Type expectedInterfaceType = typeof(IChainOperation<>).MakeGenericType(chain.ChainVariableType);
        if (!expectedInterfaceType.IsAssignableFrom(operationType)) {
            throw new InvalidOperationException($"Boot operation '{operationType.Name}' does not implement '{expectedInterfaceType.Name}'");
        }

        return operationInstance;
    }

    private static Task<IChainVariables> ExecuteAsync(Type type, object instance, IChainVariables variables, CancellationToken token) {
        if (type.GetMethod("ExecuteAsync") is not { } methodInfo) {
            throw new InvalidOperationException($"Missing ExecuteAsync method in '{type.Name}'");
        }

        // Dynamic invocation and result casting
        // I hate this, but it'll work.
        var task = (dynamic)methodInfo.Invoke(instance, [variables, token])! as Task;
        return task!.ContinueWith(t => (IChainVariables)((dynamic)t).Result, token);
    }
}