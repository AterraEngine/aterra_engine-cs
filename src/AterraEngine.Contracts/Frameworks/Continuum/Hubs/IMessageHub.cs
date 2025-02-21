// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;
using AterraEngine.Frameworks.Continuum.PipelineSteps;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageHub<in TMessageHandler, TInput, TOutput> where TMessageHandler : IMessageHandler<TInput, TOutput> {
    bool HasSubscriptions { get; }
    
    void SubscribeHandler(TMessageHandler handler);
    void AddPipelines<TPipeline>(TPipeline[] pipelines) where TPipeline : IPipelineStep<TInput, TOutput>;
    TOutput ExecuteAsync(TInput inputData, CancellationToken ct = default);
}
