// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Pipelines;
using System.Collections.Concurrent;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class MessageHub<TMessageHandler, TInput, TOutput> : IMessageHub<TInput, TOutput>
    where TMessageHandler : IMessageHandler<TInput, TOutput>
{
    
    public bool HasSubscriptions => Subscribers.Count > 0;
    protected List<TMessageHandler> Subscribers { get; } = [];
    protected ConcurrentDictionary<Guid, IPipelineStep<TInput, TOutput>> SubscribersWithPipelines { get; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddPipelines<TPipeline>(TPipeline[] pipelines) where TPipeline : IPipelineStep<TInput, TOutput> {
        int subCount = Subscribers.Count;
        if (subCount == 0) throw new InvalidOperationException("Cannot add pipeline(s) to a message hub that has no subscriber(s)");
        
        // No Original pipeline defined already
        for (int i = 0; i < subCount; i++) {
            IPipelineStep<TInput, TOutput> currentPipeline = pipelines.First();
            foreach (TPipeline pipelineStep in pipelines.Skip(1)) {
                currentPipeline.NextStep = pipelineStep.HandleStepAsync;
                currentPipeline = pipelineStep;
            }

            TMessageHandler subscriber = Subscribers[i];
            currentPipeline.NextStep = subscriber.HandleAsync;
            
            SubscribersWithPipelines.TryAdd(subscriber.Id, currentPipeline);
        }
    }
}
