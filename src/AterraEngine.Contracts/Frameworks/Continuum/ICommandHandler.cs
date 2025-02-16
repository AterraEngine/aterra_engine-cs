// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse> where TResponse : struct {
    Task StartProcessingAsync(Channel<(TCommand Command, Channel<TResponse> ReplyChannel)> channel, CancellationToken ct = default);

    ValueTask<TResponse> HandleAsync(TCommand command, CancellationToken ct = default);
}
