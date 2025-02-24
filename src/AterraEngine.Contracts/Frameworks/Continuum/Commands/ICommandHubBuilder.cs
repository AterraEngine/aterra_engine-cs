// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandHubBuilder : IMessageHubBuilder<ICommandHub>;

public interface ICommandHubBuilder<out TCommand, TResult>: ICommandHubBuilder
    where TCommand : ICommand<TResult>
    where TResult : struct {


    ICommandHubBuilder<TCommand, TResult> WithHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler<TCommand, TResult>;
    ICommandHubBuilder<TCommand, TResult> WithPipelineSteps(params Type[] types) ;
}
