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
    public FrozenDictionary<Type, ITriggerHub> TriggerHubs  { private get; init; } = FrozenDictionary<Type, ITriggerHub>.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<TOutput> ExecuteAsync<TCommand, TOutput>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TOutput> where TOutput : struct {
        if (!CommandHubs.TryGetValue(typeof(TCommand), out ICommandHub? hub)) throw new InvalidOperationException("No command hub found");
        return hub.ExecuteAsync<TCommand, TOutput>(command, ct);
    }
    public async ValueTask PublishAsync<TTrigger>(TTrigger trigger, CancellationToken ct = default) where TTrigger : ITrigger {
        if (!TriggerHubs.TryGetValue(typeof(TTrigger), out ITriggerHub? hub)) throw new InvalidOperationException("No event hub found");
        await hub.PublishAsync(trigger, ct);
    }

    public void StartProcessing() {
        IEnumerable<Task> tasks = TriggerHubs.Select(hub => hub.Value.StartProcessingAsync())
            .Concat(CommandHubs.Select(hub => hub.Value.StartProcessingAsync())); 
        
        // Fire and forget the tasks
        Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
