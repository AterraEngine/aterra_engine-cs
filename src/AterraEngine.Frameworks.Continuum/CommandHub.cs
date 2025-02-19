// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
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
    
    private readonly Channel<TOutput> _replyChannel =Channel.CreateUnbounded<TOutput>(new UnboundedChannelOptions() {
        AllowSynchronousContinuations = true,
        SingleReader = true,
        SingleWriter = false
    });

    private ICommandHandler<TCommand, TOutput>? Subscriber { get; set; }
    
    [MemberNotNullWhen(false, nameof(Subscriber))]
    public bool IsEmpty => Subscriber is null;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Subscribe<TCommandHandler>(TCommandHandler handler) where TCommandHandler : ICommandHandler<TCommand, TOutput> {
        if (!IsEmpty) throw new InvalidOperationException("Cannot subscribe to a command hub that already has a subscriber");
        Subscriber = handler;
    }
    public async Task StartProcessingAsync() => await Subscriber!.StartProcessingAsync(_channel);

    public async ValueTask<T1> PublishAsync<T0,T1>(T0 commandData, CancellationToken ct = default) where T0 : ICommand<T1> where T1 : struct {
        if (IsEmpty) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");
        if (commandData is not TCommand typedCommand) throw new ArgumentException("Command data is not of the expected type");
        if (typeof(T1) != typeof(TOutput)) throw new ArgumentException("Command data is not of the expected type");
        
        await _channel.Writer.WriteAsync((typedCommand, _replyChannel), ct);
        while (await _replyChannel.Reader.WaitToReadAsync(ct)) {
            if (_replyChannel.Reader.TryRead(out TOutput result) && result is T1 castedResult) return castedResult;
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
