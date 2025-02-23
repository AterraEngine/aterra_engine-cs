// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandBuilder<TCommand, TResult> : ICommandBuilder<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct 
{
    public Type CommandType { get; } = typeof(TCommand);
    public Type ReturnType { get; } = typeof(TResult);
    
    public Type GetCommandHubType() => typeof(ICommandHub<,>).MakeGenericType(CommandType, ReturnType);

    public Type? CommandHandlerType { get; private set; }
    public Type[] PipelineSteps { get; private set; } = [];


    public ICommandBuilder<TCommand, TResult> WithHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler<TCommand, TResult> {
        CommandHandlerType = typeof(TCommandHandler);
        return this;
    }
    
    public ICommandBuilder<TCommand, TResult> WithPipelineSteps(params Type[] types) {
        PipelineSteps = PipelineSteps.Concat(types).ToArray();
        return this;
    }
}
