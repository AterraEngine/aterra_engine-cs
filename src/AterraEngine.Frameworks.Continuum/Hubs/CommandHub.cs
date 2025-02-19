// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandHub<TCommand, TOutput> : MessageHub<IMessageHandler<TCommand, ValueTask<TOutput>>, TCommand, ValueTask<TOutput>>, ICommandHub<TCommand, TOutput> 
    where TCommand : ICommand<TOutput> where TOutput : struct 
{
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
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Subscribe<TCommandHandler>(TCommandHandler handler) where TCommandHandler : ICommandHandler<TCommand, TOutput> {
        if (HasSubscriptions) throw new InvalidOperationException("Cannot subscribe to a command hub that already has a subscriber");
        Subscribers.Add(handler);
    }
    
    public async Task StartProcessingAsync() {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot start processing a command hub that has no subscriber");
        
        while (await _channel.Reader.WaitToReadAsync()) {
            while (_channel.Reader.TryRead(out (TCommand Command, Channel<TOutput> ReplyChannel) data)) {
                // Each handle should be their own CancellationToken.
                // But there should be a way to define how much this is depending on some sort of config?
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                IMessageHandler<TCommand, ValueTask<TOutput>> subscriber = Subscribers.First();
                
                TOutput result = SubscribersWithPipelines.IsEmpty ? 
                    await subscriber.HandleAsync(data.Command, cts.Token) : 
                    await SubscribersWithPipelines[subscriber.Id].HandleStepAsync(data.Command, cts.Token);

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
}
