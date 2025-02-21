// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBus {
    ValueTask<TResponse> QueryAsync<TQuery, TResponse>(TQuery query, CancellationToken ct = default) where TQuery : IQuery<TResponse> where TResponse : struct;
    ValueTask<TOutput> ExecuteAsync<TCommand, TOutput>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TOutput> where TOutput : struct;
    ValueTask PublishAsync<TTrigger>(TTrigger trigger, CancellationToken ct = default) where TTrigger : ITrigger;
    
    void StartProcessing();
}
