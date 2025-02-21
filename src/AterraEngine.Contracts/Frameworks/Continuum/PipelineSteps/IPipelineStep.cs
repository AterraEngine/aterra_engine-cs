// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;

namespace AterraEngine.Frameworks.Continuum.PipelineSteps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPipelineStep<TInput, TOutput> : IMessageHandler<TInput, TOutput> {
    Func<TInput, CancellationToken, TOutput> NextStep { set; }
}
