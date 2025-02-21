// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Hubs;
using System.Collections.Frozen;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MessageBus : IMessageBus {
    public FrozenDictionary<Type, ICommandHub> CommandHubs { private get; init; } = FrozenDictionary<Type, ICommandHub>.Empty;
    public FrozenDictionary<Type, ITriggerHub> TriggerHubs { private get; init; } = FrozenDictionary<Type, ITriggerHub>.Empty;
    public FrozenDictionary<Type, IQueryHub> QueryHubs { private get; init; } = FrozenDictionary<Type, IQueryHub>.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<TResponse> QueryAsync<TQuery, TResponse>(TQuery query, CancellationToken ct = default) where TQuery : IQuery<TResponse> where TResponse : struct {
        if (!QueryHubs.TryGetValue(typeof(TQuery), out IQueryHub? hub)) throw new InvalidOperationException("No command hub found");
        if (hub is not IQueryHub<TQuery, TResponse> castedHub) throw new InvalidOperationException("Command hub is not of the expected type");

        return await castedHub.ExecuteAsync(query, ct);
    }
    public ValueTask<TOutput> ExecuteAsync<TCommand, TOutput>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TOutput> where TOutput : struct {
        if (!CommandHubs.TryGetValue(typeof(TCommand), out ICommandHub? hub)) throw new InvalidOperationException("No command hub found");
        if (hub is not ICommandHub<TCommand, TOutput> castedHub) throw new InvalidOperationException("Command hub is not of the expected type");

        return castedHub.ExecuteAsync(command, ct);
    }

    public async ValueTask PublishAsync<TTrigger>(TTrigger trigger, CancellationToken ct = default) where TTrigger : ITrigger {
        if (!TriggerHubs.TryGetValue(typeof(TTrigger), out ITriggerHub? hub)) throw new InvalidOperationException("No trigger hub found");
        if (hub is not ITriggerHub<TTrigger> castedHub) throw new InvalidOperationException("Trigger hub is not of the expected type");

        await castedHub.ExecuteAsync(trigger, ct);
    }

    public void StartProcessing() => _ = StartProcessingAsync().ConfigureAwait(false);

    private async Task StartProcessingAsync() {

        var tasks = new Task[TriggerHubs.Count + CommandHubs.Count + QueryHubs.Count];

        // This is scuffed, but works, so hey what do we care.
        int i;
        int j;
        int k;
        for (i = 0; i < TriggerHubs.Count + 0; i++) {
            tasks[i] = TriggerHubs.Values[i].StartProcessingAsync();
        }

        for (j = i; j < CommandHubs.Count + i; j++) {
            tasks[j] = CommandHubs.Values[j - i].StartProcessingAsync();
        }

        for (k = j; k < QueryHubs.Count + j; k++) {
            tasks[k] = CommandHubs.Values[k - j].StartProcessingAsync();
        }

        // Fire and forget the tasks
        await Task.WhenAll(tasks);
    }
}
