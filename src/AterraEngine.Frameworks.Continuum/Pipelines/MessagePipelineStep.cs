// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.Pipelines;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class MessagePipelineStep<TInput, TOutput> : IPipelineStep<TInput, TOutput> {
    public bool IsNextStepPopulated => NextStep != null;
    public Func<TInput, CancellationToken, TOutput>? NextStep { get; set; }
    
    public abstract TOutput HandleStepAsync(TInput input, CancellationToken ct = default);
}
