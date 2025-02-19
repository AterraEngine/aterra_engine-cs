// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Pipelines;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageHub<TInput, TOutput> {
    bool HasSubscriptions { get; }

    void AddPipelines<TPipeline>(TPipeline[] pipelines) where TPipeline : IPipelineStep<TInput, TOutput>;
}
