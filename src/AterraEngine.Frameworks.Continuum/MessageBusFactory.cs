// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// TODO Fully rework factory pattern to just save all the types, and resolve on Create()
public class MessageBusFactory(IScopedProvider provider) : IMessageBusFactory {
    private readonly ConcurrentDictionary<Type, ICommandBuilder> _commandHubs = [];
    private readonly ConcurrentDictionary<Type, ITriggerBuilder> _triggerHubs = [];
    private readonly ConcurrentDictionary<Type, IQueryBuilder> _queryHubs = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IContinuum Create(IScopedProvider provider) {
        return new MessageBus {
            CommandHubs = _commandHubs.ToFrozenDictionary(
                kvp => kvp.Key,
                kvp => {
                    var builder = kvp.Value;
                    var hub = provider.GetRequiredService(builder.GetCommandHubType()) as ICommandHub;
                    
                    // subscribe has to be handled a bit differently now then
                    hub.Subscribe(builder.ReturnType);
                }),
            TriggerHubs = _registeredTriggerHubs.ToFrozenDictionary(),
            QueryHubs = _registeredQueryHubs.ToFrozenDictionary()
        };
    }

    public ICommandBuilder<TCommand, TResult> AddCommand<TCommand, TResult>() 
        where TCommand : ICommand<TResult>
        where TResult : struct 
    {
        ICommandBuilder builder = _commandHubs.GetOrAdd(
            typeof(TCommand),
            static _ => new CommandBuilder<TCommand, TResult>());
        
        if (builder is not ICommandBuilder<TCommand, TResult> typedBuilder) throw new InvalidOperationException("Failed to get command builder");
        
        return typedBuilder;
    }


    public ITriggerBuilder<TTrigger> AddTrigger<TTrigger>() where TTrigger : ITrigger {
        ITriggerBuilder builder = _triggerHubs.GetOrAdd(
            typeof(TTrigger),
            static _ => new TriggerBuilder<TTrigger>());
        
        if (builder is not ITriggerBuilder<TTrigger> typedBuilder) throw new InvalidOperationException("Failed to get typed builder");
        
        return typedBuilder;
    }

    public IQueryBuilder<TQuery, TResult> AddQuery<TQuery, TResult>() where TQuery : IQuery<TResult> where TResult : struct {
        IQueryBuilder builder = _queryHubs.GetOrAdd(
            typeof(TQuery),
            static _ => new QueryBuilder<TQuery, TResult>());
        
        if (builder is not IQueryBuilder<TQuery, TResult> typedBuilder) throw new InvalidOperationException("Failed to get typed builder");
        
        return typedBuilder;
    }
}
