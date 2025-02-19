// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
    public void Subscribe<TTriggerHandler>(TTriggerHandler handler) where TTriggerHandler : ITriggerHandler<TTrigger> {
        Subscribers.Add(handler);
    }

    public async ValueTask PublishAsync<T>(T eventData, CancellationToken ct = default) where T : ITrigger {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");
        if (eventData is not TTrigger typedTrigger) throw new ArgumentException("Command data is not of the expected type");
        if (typeof(T) != typeof(TTrigger)) throw new ArgumentException("Command data is not of the expected type");
        
        await _channel.Writer.WriteAsync(typedTrigger, ct);
    }
    
    public async Task StartProcessingAsync() {
        while (await _channel.Reader.WaitToReadAsync()) {
            while (_channel.Reader.TryRead(out TTrigger? trigger)) {
                // Each handle should be their own CancellationToken.
                // But there should be a way to define how much this is depending on some sort of config?
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                
                IEnumerable<Task> tasks = SubscribersWithPipelines.IsEmpty
                    ? Subscribers.Select(subscriber => subscriber.HandleAsync(trigger, cts.Token)) 
                    : SubscribersWithPipelines.Values.Select(sub => sub.HandleStepAsync(trigger, cts.Token));
                
                await Task.WhenAll(tasks);
            }
        }
    }
}
