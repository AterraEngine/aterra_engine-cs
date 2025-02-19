// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse> where TResponse : struct {
    ValueTask<TResponse> HandleAsync(TCommand command, CancellationToken ct = default);
}
