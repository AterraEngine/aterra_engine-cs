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
public class MessageBusFactory : IMessageBusFactory {
    public readonly ConcurrentDictionary<Type, ICommandHub> RegisteredCommandHubs = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IMessageBus Create(IScopedProvider provider) {
        return new MessageBus {
            CommandHubs = RegisteredCommandHubs.ToFrozenDictionary(),
        };
    }
}
