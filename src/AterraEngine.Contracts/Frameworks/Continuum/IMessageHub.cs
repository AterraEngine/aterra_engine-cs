// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageHub {
    bool HasSubscriptions { get; }

    Task StartProcessingAsync();
}

public interface IMessageHub<in TMessageHandler, TInput, TOutput> : IMessageHub where TMessageHandler : IMessageHandler<TInput, TOutput> {
    void SubscribeHandler(TMessageHandler handler);
    void AddPipelines<TPipeline>(TPipeline[] pipelines) where TPipeline : IPipelineStep<TInput, TOutput>;

    TOutput ExecuteAsync(TInput inputData, CancellationToken ct = default);
}
