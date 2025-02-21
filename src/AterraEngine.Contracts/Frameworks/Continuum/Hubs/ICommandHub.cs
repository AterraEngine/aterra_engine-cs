// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandHub {
    Task StartProcessingAsync();
}

public interface ICommandHub<TCommand, TOutput> : ICommandHub, IMessageHub<IMessageHandler<TCommand, ValueTask<TOutput>>, TCommand, ValueTask<TOutput>>
    where TCommand : ICommand<TOutput>
    where TOutput : struct;