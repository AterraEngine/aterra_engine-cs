// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.Pipelines;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class CommandPipeline<TCommand, TResult> : ICommandPipelineStep<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct {
    
    public Func<TCommand, CancellationToken, ValueTask<TResult>> NextStep { protected get; set; } = null!;

    public abstract ValueTask<TResult> HandleStepAsync(TCommand command, CancellationToken ct = default);
}
