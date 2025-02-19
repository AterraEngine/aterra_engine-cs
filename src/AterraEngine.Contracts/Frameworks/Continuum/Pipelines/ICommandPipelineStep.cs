// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.Pipelines;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICommandPipelineStep<TCommand, TResult> : IPipelineStep<Func<TCommand, CancellationToken, ValueTask<TResult>>>
    where TCommand : ICommand<TResult>
    where TResult : struct
{
    public ValueTask<TResult> HandleStepAsync(TCommand command, CancellationToken ct = default);
}
