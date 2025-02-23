// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandBuilder {
    Type CommandType { get; }
    Type ReturnType { get; }

    Type GetCommandHubType();
}

public interface ICommandBuilder<out TCommand, TResult>: ICommandBuilder
    where TCommand : ICommand<TResult>
    where TResult : struct {


    ICommandBuilder<TCommand, TResult> WithHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler<TCommand, TResult>;
    ICommandBuilder<TCommand, TResult> WithPipelineSteps(params Type[] types) ;
}
