// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Threading.Channels;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class CommandHandler<TCommand, TOutput>() : ICommandHandler<TCommand, TOutput> where TCommand : ICommand<TOutput> where TOutput : struct {
    public async Task StartProcessingAsync(Channel<(TCommand Command, Channel<TOutput> ReplyChannel)> channel) {
        while (await channel.Reader.WaitToReadAsync()) {
            while (channel.Reader.TryRead(out (TCommand Command, Channel<TOutput> ReplyChannel) data)) {
                TOutput result = await HandleAsync(data.Command);
                await data.ReplyChannel.Writer.WriteAsync(result);
            }
        }
    }
    
    public abstract ValueTask<TOutput> HandleAsync(TCommand command) ;
}
