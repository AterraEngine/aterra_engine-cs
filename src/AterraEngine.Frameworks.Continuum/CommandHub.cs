// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Pipelines;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandHub<TCommand, TOutput> : ICommandHub<TCommand, TOutput> where TCommand : ICommand<TOutput> where TOutput : struct {
    private readonly Channel<(TCommand Command, Channel<TOutput> ReplyChannel)> _channel = Channel.CreateUnbounded<(TCommand Command, Channel<TOutput> ReplyChannel)>(new UnboundedChannelOptions() {
        AllowSynchronousContinuations = true,
        SingleReader = true,
        SingleWriter = false
    });
    
    private readonly Channel<TOutput> _replyChannel = Channel.CreateUnbounded<TOutput>(new UnboundedChannelOptions() {
        AllowSynchronousContinuations = true,
        SingleReader = true,
        SingleWriter = false
    });

    private ICommandHandler<TCommand, TOutput>? Subscriber { get; set; }
    private ICollection<ICommandPipelineStep<TCommand, TOutput>> Pipelines { get; set; } = []; 
    private ICommandPipelineStep<TCommand, TOutput>? PipelineOrigin { get; set; }
    
    [MemberNotNullWhen(true, nameof(Subscriber))]
    public bool HasSubscriptions => Subscriber is not null;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Subscribe<TCommandHandler>(TCommandHandler handler) where TCommandHandler : ICommandHandler<TCommand, TOutput> {
        if (HasSubscriptions) throw new InvalidOperationException("Cannot subscribe to a command hub that already has a subscriber");
        Subscriber = handler;
    }

    public void AddPipelineStep<TPipeline>(TPipeline pipeline) where TPipeline : ICommandPipelineStep<TCommand, TOutput> {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot add pipeline to a command hub that has no subscriber");
        Pipelines.Add(pipeline);
        
        // No Original pipeline defined already
        if (PipelineOrigin is null) {
            PipelineOrigin = pipeline;
            PipelineOrigin.NextStep = Subscriber.HandleAsync;
            return;
        }
                    
        // Assign all pipelines to be sequentially set after each other 
        ICommandPipelineStep<TCommand, TOutput> currentPipeline = PipelineOrigin;
        foreach (ICommandPipelineStep<TCommand, TOutput> pipelineStep in Pipelines.Skip(1)) {
            currentPipeline.NextStep = pipelineStep.HandleStepAsync;
            currentPipeline = pipelineStep;
        }
                    
        // Final step is to actually execute the Handler
        pipeline.NextStep = Subscriber.HandleAsync;
    }
    
    
    public async Task StartProcessingAsync() {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot start processing a command hub that has no subscriber");
        
        while (await _channel.Reader.WaitToReadAsync()) {
            while (_channel.Reader.TryRead(out (TCommand Command, Channel<TOutput> ReplyChannel) data)) {
                // Each handle should be their own CancellationToken.
                // But there should be a way to define how much this is depending on some sort of config?
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                TOutput result = PipelineOrigin is not null 
                    ? await PipelineOrigin.HandleStepAsync(data.Command, cts.Token)
                    : await Subscriber.HandleAsync(data.Command, cts.Token);

                await data.ReplyChannel.Writer.WriteAsync(result, cts.Token);
            }
        }
    }

    public async ValueTask<T1> ExecuteAsync<T0,T1>(T0 commandData, CancellationToken ct = default) where T0 : ICommand<T1> where T1 : struct {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");
        if (commandData is not TCommand typedCommand) throw new ArgumentException("Command data is not of the expected type");
        if (typeof(T1) != typeof(TOutput)) throw new ArgumentException("Command data is not of the expected type");
        
        await _channel.Writer.WriteAsync((typedCommand, _replyChannel), ct);
        while (await _replyChannel.Reader.WaitToReadAsync(ct)) {
            if (_replyChannel.Reader.TryRead(out TOutput result) || result is not T1 castedResult) continue;
            return castedResult;
        }
        throw new InvalidOperationException("No reply was received");
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static CommandHub<TCommand, TOutput> FromHandler<TCommandHandler>(TCommandHandler handler) where TCommandHandler : ICommandHandler<TCommand, TOutput> {
        var hub = new CommandHub<TCommand, TOutput>();
        hub.Subscribe(handler);
        return hub;
    }
}
