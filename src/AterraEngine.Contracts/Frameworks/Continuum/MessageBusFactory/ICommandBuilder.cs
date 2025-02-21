// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandBuilder<out TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct {


    ICommandBuilder<TCommand, TResult> WithHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler<TCommand, TResult>;
    ICommandBuilder<TCommand, TResult> WithPipelineSteps(params Type[] types) ;
}
