// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MessageBus : IMessageBus {
    public FrozenDictionary<Type, ICommandHub> CommandHubs  { private get; init; } = FrozenDictionary<Type, ICommandHub>.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<TOutput> ExecuteAsync<TCommand, TOutput>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TOutput> where TOutput : struct {
        if (!CommandHubs.TryGetValue(typeof(TCommand), out ICommandHub? hub)) throw new InvalidOperationException("No command hub found");
        return hub.PublishAsync<TCommand, TOutput>(command, ct);
    }

    public void StartProcessingAsync() {
        foreach ((_, ICommandHub hub) in CommandHubs) {
            _ = hub.StartProcessingAsync(); // Fire-and-forget
        }
    }
}
