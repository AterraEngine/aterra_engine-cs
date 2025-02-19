// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandBuilder<out TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct {


    ICommandBuilder<TCommand, TResult> AddHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler<TCommand, TResult>;
}
