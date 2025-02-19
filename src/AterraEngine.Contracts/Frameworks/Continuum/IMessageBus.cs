// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBus {
    ValueTask<TOutput> ExecuteAsync<TCommand, TOutput>(TCommand command, CancellationToken ct = default) where TCommand : ICommand<TOutput> where TOutput : struct;
    ValueTask PublishAsync<TTrigger>(TTrigger triger, CancellationToken ct = default) where TTrigger : ITrigger;
    
    void StartProcessing();
}
