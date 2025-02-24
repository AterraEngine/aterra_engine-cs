// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandHub : IMessageHub;

public interface ICommandHub<TCommand, TOutput> : ICommandHub, IMessageHub<IMessageHandler<TCommand, ValueTask<TOutput>>, TCommand, ValueTask<TOutput>>
    where TCommand : ICommand<TOutput>
    where TOutput : struct;
