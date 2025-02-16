// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class CommandHandler<TCommand, TOutput> : ICommandHandler<TCommand, TOutput> where TCommand : ICommand<TOutput> where TOutput : struct {
    public async Task StartProcessingAsync(Channel<(TCommand Command, Channel<TOutput> ReplyChannel)> channel, CancellationToken ct = default) {
        while (await channel.Reader.WaitToReadAsync(ct)) {
            while (channel.Reader.TryRead(out (TCommand Command, Channel<TOutput> ReplyChannel) data)) {
                // Each handle should be their own CancellationToken.
                // But there should be a way to define how much this is depending on some sort of config?
                TOutput result = await HandleAsync(data.Command, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
                await data.ReplyChannel.Writer.WriteAsync(result, ct);
            }
        }
    }
    
    public abstract ValueTask<TOutput> HandleAsync(TCommand command, CancellationToken ct = default);
}
