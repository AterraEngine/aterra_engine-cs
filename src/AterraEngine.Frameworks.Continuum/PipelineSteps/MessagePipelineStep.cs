// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum.PipelineSteps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class MessagePipelineStep<TInput, TOutput> : IPipelineStep<TInput, TOutput> {
    public Func<TInput, CancellationToken, TOutput> NextStep { get; set; } = null!;
    public Guid Id { get; } = Guid.CreateVersion7();
    public abstract TOutput HandleAsync(TInput input, CancellationToken ct = default);
}
