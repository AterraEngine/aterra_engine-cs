// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContinuumService : IContinuum {
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

    public async ValueTask TriggerAsync<TTrigger>(TTrigger trigger, CancellationToken ct = default) where TTrigger : ITrigger {
        if (!TriggerHubs.TryGetValue(typeof(TTrigger), out ITriggerHub? hub)) throw new InvalidOperationException("No trigger hub found");
        if (hub is not ITriggerHub<TTrigger> castedHub) throw new InvalidOperationException("Trigger hub is not of the expected type");

        await castedHub.ExecuteAsync(trigger, ct);
    }

    public void StartProcessing() => _ = StartProcessingAsync().ConfigureAwait(false);

    private async Task StartProcessingAsync() {
        await Task.WhenAll([
            ..TriggerHubs.Values.Select(x => x.StartProcessingAsync()),
            ..CommandHubs.Values.Select(x => x.StartProcessingAsync())
        ]);
    }
}
