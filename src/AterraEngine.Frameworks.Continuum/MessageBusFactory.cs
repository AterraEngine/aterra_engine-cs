// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Continuum.Hubs;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MessageBusFactory(IScopedProvider provider) : IMessageBusFactory {
    private readonly ConcurrentDictionary<Type, ICommandHub> _registeredCommandHubs = [];
    private readonly ConcurrentDictionary<Type, ITriggerHub> _registeredTriggerHubs = [];
    private readonly ConcurrentDictionary<Type, IQueryHub> _registeredQueryHubs = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IMessageBus Create(IScopedProvider _) {
        return new MessageBus {
            CommandHubs = _registeredCommandHubs.ToFrozenDictionary(),
            TriggerHubs = _registeredTriggerHubs.ToFrozenDictionary(),
            QueryHubs = _registeredQueryHubs.ToFrozenDictionary()
        };
    }

    public ICommandBuilder<TCommand, TResult> AddCommand<TCommand, TResult>() 
        where TCommand : ICommand<TResult>
        where TResult : struct 
    {
        ICommandHub hub = _registeredCommandHubs.GetOrAdd(
            typeof(TCommand),
            static (_, provider) => provider.GetRequiredService<ICommandHub<TCommand, TResult>>(),
            provider
        );
        
        if (hub is not ICommandHub<TCommand, TResult> typedHub) throw new InvalidOperationException("Failed to get command hub");
        
        return new CommandBuilder<TCommand, TResult>(typedHub, provider);
    }


    public ITriggerBuilder<TTrigger> AddTrigger<TTrigger>() where TTrigger : ITrigger {
        // One trigger hub can have multiple subscribers
        ITriggerHub hub = _registeredTriggerHubs.GetOrAdd(
            typeof(TTrigger),
            static (_, provider) => provider.GetRequiredService<ITriggerHub<TTrigger>>(),
            provider
        );
        if (hub is not ITriggerHub<TTrigger> typedHub) throw new InvalidOperationException("Failed to get trigger hub");
        
        return new TriggerBuilder<TTrigger>(typedHub, provider);
    }

    public IQueryBuilder<TQuery, TResult> AddQuery<TQuery, TResult>() where TQuery : IQuery<TResult> where TResult : struct {
        IQueryHub hub = _registeredQueryHubs.GetOrAdd(
            typeof(TQuery),
            static (_, provider) => provider.GetRequiredService<IQueryHub<TQuery, TResult>>(),
            provider
        );
        
        if (hub is not IQueryHub<TQuery, TResult> typedHub) throw  new InvalidOperationException("Failed to get command hub");
        return new QueryBuilder<TQuery, TResult>(typedHub, provider);
    }
}
