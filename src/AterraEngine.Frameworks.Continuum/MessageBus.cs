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

    public void StartProcessing() => _ = StartProcessingAsync().ConfigureAwait(false);
    
    private async Task StartProcessingAsync() {
        
        var tasks = new Task[TriggerHubs.Count + CommandHubs.Count];

        // This is scuffed, but works, so hey what do we care.
        int i;
        for (i = 0; i < TriggerHubs.Count; i++) {
            tasks[i] = TriggerHubs.Values[i].StartProcessingAsync();
        }

        for (int j = i; j < CommandHubs.Count + i ; j++) {
            tasks[j] = CommandHubs.Values[j - i].StartProcessingAsync();
        }
        
        // Fire and forget the tasks
        await Task.WhenAll(tasks);
    }
}
