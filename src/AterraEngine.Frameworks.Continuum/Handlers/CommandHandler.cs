// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.Handlers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class CommandHandler<TCommand, TResponse> : MessageHandler<TCommand, ValueTask<TResponse>>, ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : struct;

