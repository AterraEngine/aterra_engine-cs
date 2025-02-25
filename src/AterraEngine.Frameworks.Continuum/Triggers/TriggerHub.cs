// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TriggerHub<TTrigger> : MessageHub<ITriggerHandler<TTrigger>, TTrigger, Task>, ITriggerHub<TTrigger>
    where TTrigger : ITrigger {
    private readonly Channel<TriggerHubChannelDto<TTrigger>> _channel = Channel.CreateUnbounded<TriggerHubChannelDto<TTrigger>>(new UnboundedChannelOptions {
        AllowSynchronousContinuations = true,
        SingleReader = false,
        SingleWriter = false
    });

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task StartProcessingAsync() {
        while (await _channel.Reader.WaitToReadAsync()) {
            while (_channel.Reader.TryRead(out TriggerHubChannelDto<TTrigger>? dto)) {
                dto.CancellationToken.ThrowIfCancellationRequested();
                
                Span<ITriggerHandler<TTrigger>> subscribers = GetSubscribers();
                var tasks = new Task[SubscriberCount];
                
                for (int i = Subscribers.Count - 1; i >= 0; i--) {
                    tasks[i] = subscribers[i].HandleAsync(dto.Trigger, dto.CancellationToken);
                }
                
                await Task.WhenAll(tasks);
            }
        }
    }

    public override async Task ExecuteAsync(TTrigger inputData, CancellationToken ct = default) {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");

        var dto = new TriggerHubChannelDto<TTrigger>(inputData, ct);
        await _channel.Writer.WriteAsync(dto, ct);
    }
}
