// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.Pipelines;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class CommandPipeline<TCommand, TResult> : MessagePipelineStep<TCommand, ValueTask<TResult>>, ICommandPipelineStep<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct;
