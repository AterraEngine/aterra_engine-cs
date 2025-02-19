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
public class MessageBusFactory(IScopedProvider provider) : IMessageBusFactory {
    private readonly ConcurrentDictionary<Type, ICommandHub> _registeredCommandHubs = [];
    private readonly ConcurrentDictionary<Type, ITriggerHub> _registeredTriggerHubs = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IMessageBus Create(IScopedProvider _) {
        return new MessageBus {
            CommandHubs = _registeredCommandHubs.ToFrozenDictionary(),
            TriggerHubs = _registeredTriggerHubs.ToFrozenDictionary()
        };
    }

    public IMessageBusFactory AddCommand<TCommandHandler, TCommand, TResult>() 
        where TCommand : ICommand<TResult>
        where TResult : struct 
        where TCommandHandler : class, ICommandHandler<TCommand, TResult>
    {
        var hub = provider.GetRequiredService<ICommandHub<TCommand, TResult>>();
        var handler = provider.GetRequiredService<TCommandHandler>();
        
        hub.Subscribe(handler);
        
        _registeredCommandHubs.TryAdd(typeof(TCommand),hub);
        return this;
    }
    
    public IMessageBusFactory AddTrigger<TTriggerHandler, TTrigger>() where TTrigger : ITrigger where TTriggerHandler : class, ITriggerHandler<TTrigger> {
        // One trigger hub can have multiple subscribers
        ITriggerHub hub = _registeredTriggerHubs.GetOrAdd(
            typeof(TTrigger),
            static (_, provider) => provider.GetRequiredService<ITriggerHub<TTrigger>>(),
            provider
        );
        if (hub is not ITriggerHub<TTrigger> typedHub) throw new InvalidOperationException("Failed to get trigger hub");
        
        var handler = provider.GetRequiredService<TTriggerHandler>();

        typedHub.Subscribe(handler);
        return this;
    }
}
