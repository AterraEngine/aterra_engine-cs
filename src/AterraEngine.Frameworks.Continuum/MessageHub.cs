// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class MessageHub<TMessageHandler, TInput, TOutput> : IMessageHub<TMessageHandler, TInput, TOutput>
    where TMessageHandler : class, IMessageHandler<TInput, TOutput> {
    protected int SubscriberCount => Subscribers.Count;

    private List<Guid> SubscriberOrder { get; } = [];
    protected ConcurrentDictionary<Guid, IMessageHandler<TInput, TOutput>> Subscribers { get; } = [];

    public bool HasSubscriptions => !Subscribers.IsEmpty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract Task StartProcessingAsync();

    public virtual void SubscribeHandler(TMessageHandler handler) {
        SubscriberOrder.Add(handler.Id);
        Subscribers.TryAdd(handler.Id, handler);
    }

    public void AddPipelines<TPipeline>(TPipeline[] pipelines) where TPipeline : IPipelineStep<TInput, TOutput> {
        ArgumentNullException.ThrowIfNull(pipelines);
        if (pipelines.Length == 0) return;

        int subCount = SubscriberOrder.Count;
        if (subCount == 0) throw new InvalidOperationException("Cannot add pipeline(s) to a message hub that has no subscriber(s)");

        // No Original pipeline defined already
        for (int i = 0; i < subCount; i++) {
            IPipelineStep<TInput, TOutput> currentPipeline = pipelines.First();
            foreach (TPipeline pipelineStep in pipelines.Skip(1)) {
                currentPipeline.NextStep = pipelineStep;
                currentPipeline = pipelineStep;
            }

            Guid id = SubscriberOrder[i];
            IMessageHandler<TInput, TOutput> originalSubscriber = Subscribers[id];
            currentPipeline.NextStep = originalSubscriber;

            Subscribers.AddOrUpdate(id, currentPipeline);
        }
    }
    public abstract TOutput ExecuteAsync(TInput inputData, CancellationToken ct = default);

    protected Span<TMessageHandler> GetSubscribers() {
        var subscribers = new TMessageHandler[SubscriberCount];
        for (int i = SubscriberCount - 1; i >= 0; i--) {
            subscribers[i] = Subscribers[SubscriberOrder[i]] as TMessageHandler ?? throw new InvalidOperationException("Subscriber is not of the expected type");
        }
        return subscribers;
    }
}
