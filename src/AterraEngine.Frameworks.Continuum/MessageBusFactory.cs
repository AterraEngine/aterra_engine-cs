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
    public readonly ConcurrentDictionary<Type, ICommandHub> RegisteredCommandHubs = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IMessageBus Create(IScopedProvider _) {
        return new MessageBus {
            CommandHubs = RegisteredCommandHubs.ToFrozenDictionary()
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
        
        RegisteredCommandHubs.TryAdd(typeof(TCommand),hub);
        return this;
    }
}
