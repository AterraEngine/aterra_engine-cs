// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.PipelineSteps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class CommandPipelineStep<TCommand, TResult> : MessagePipelineStep<TCommand, ValueTask<TResult>>, ICommandPipelineStep<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct;
