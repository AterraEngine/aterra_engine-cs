// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;
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
        SingleReader = false,
        SingleWriter = false
    });
    
    private readonly Channel<TOutput> _replyChannel = Channel.CreateUnbounded<TOutput>(new UnboundedChannelOptions() {
        AllowSynchronousContinuations = true,
        SingleReader = false,
        SingleWriter = false
    });
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void SubscribeHandler(IMessageHandler<TCommand, ValueTask<TOutput>> handler) {
        if (SubscriberCount != 0) throw new InvalidOperationException("Cannot subscribe to a command hub that already has a subscriber");
        base.SubscribeHandler(handler);
    }

    public async Task StartProcessingAsync() {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot start processing a command hub that has no subscriber");
        
        while (await _channel.Reader.WaitToReadAsync()) {
            // TODO Why is there only one reply channel per hub?
            while (_channel.Reader.TryRead(out (TCommand Command, Channel<TOutput> ReplyChannel) data)) {
                // Each handle should be their own CancellationToken.
                // But there should be a way to define how much this is depending on some sort of config?
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                IMessageHandler<TCommand, ValueTask<TOutput>> subscriber = GetSubscribers()[0];
                
                TOutput result = await subscriber.HandleAsync(data.Command, cts.Token);

                await data.ReplyChannel.Writer.WriteAsync(result, cts.Token);
            }
        }
    }
    
    public async override ValueTask<TOutput> ExecuteAsync(TCommand inputData, CancellationToken ct = default) {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");
        
        await _channel.Writer.WriteAsync((inputData, _replyChannel), ct);
        while (await _replyChannel.Reader.WaitToReadAsync(ct)) {
            // Can only receive ONE reply per request, so we should do it like this 
            if (!_replyChannel.Reader.TryRead(out TOutput result)) continue;
            return result;
        }
        throw new InvalidOperationException("No reply was received");
    }
}
