// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TriggerHub<TTrigger> : MessageHub<ITriggerHandler<TTrigger>, TTrigger, Task>, ITriggerHub<TTrigger>
    where TTrigger : ITrigger 
{
    private readonly Channel<TTrigger> _channel = Channel.CreateUnbounded<TTrigger>(new UnboundedChannelOptions() {
        AllowSynchronousContinuations = true,
        SingleReader = false,
        SingleWriter = false
    });

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task StartProcessingAsync() {
        while (await _channel.Reader.WaitToReadAsync()) {
            while (_channel.Reader.TryRead(out TTrigger? trigger)) {
                // Each handle should be their own CancellationToken.
                // But there should be a way to define how much this is depending on some sort of config?
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                
                Span<ITriggerHandler<TTrigger>> subscribers = GetSubscribers();
                var tasks = new Task[SubscriberCount];
                
                for (int i = Subscribers.Count - 1; i >= 0; i--) {
                    tasks[i] = subscribers[i].HandleAsync(trigger, cts.Token);
                }
                
                await Task.WhenAll(tasks);
            }
        }
    }
    
    public async override Task ExecuteAsync(TTrigger inputData, CancellationToken ct = default) {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");
        await _channel.Writer.WriteAsync(inputData, ct);
    }
}
