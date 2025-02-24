// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandHub<TCommand, TOutput> : MessageHub<IMessageHandler<TCommand, ValueTask<TOutput>>, TCommand, ValueTask<TOutput>>, ICommandHub<TCommand, TOutput>
    where TCommand : ICommand<TOutput> where TOutput : struct {
    private readonly Channel<CommandHubChannelDto<TCommand, TOutput>> _channel = Channel.CreateUnbounded<CommandHubChannelDto<TCommand, TOutput>>(new UnboundedChannelOptions {
        AllowSynchronousContinuations = true,
        SingleReader = false,
        SingleWriter = false
    });

    private readonly Channel<TOutput> _replyChannel = Channel.CreateUnbounded<TOutput>(new UnboundedChannelOptions {
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

    public override async Task StartProcessingAsync() {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot start processing a command hub that has no subscriber");

        while (await _channel.Reader.WaitToReadAsync()) {
            while (_channel.Reader.TryRead(out CommandHubChannelDto<TCommand, TOutput>? dto)) {
                IMessageHandler<TCommand, ValueTask<TOutput>> subscriber = GetSubscribers()[0];
                TOutput result = await subscriber.HandleAsync(dto.CommandData, dto.CancellationToken);

                await dto.ReplyChannel.Writer.WriteAsync(result, dto.CancellationToken);
            }
        }
    }

    public override async ValueTask<TOutput> ExecuteAsync(TCommand inputData, CancellationToken ct = default) {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");

        var dto = new CommandHubChannelDto<TCommand, TOutput>(inputData, _replyChannel, ct);
        await _channel.Writer.WriteAsync(dto, ct);

        while (await _replyChannel.Reader.WaitToReadAsync(ct)) {
            // Can only receive ONE reply per request, so we should do it like this 
            if (!_replyChannel.Reader.TryRead(out TOutput result)) continue;
            return result;
        }
        throw new InvalidOperationException("No reply was received");
    }
}
