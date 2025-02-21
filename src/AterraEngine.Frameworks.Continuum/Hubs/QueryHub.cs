// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class QueryHub<TQuery, TOutput> : MessageHub<IMessageHandler<TQuery, ValueTask<TOutput>>, TQuery, ValueTask<TOutput>>, IQueryHub<TQuery, TOutput> 
    where TQuery : IQuery<TOutput> where TOutput : struct 
{
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override Task StartProcessingAsync() => Task.CompletedTask;
    
    public async override ValueTask<TOutput> ExecuteAsync(TQuery inputData, CancellationToken ct = default) {
        if (!HasSubscriptions) throw new InvalidOperationException("Cannot publish to a command hub that has no subscriber");
        
        // One Query handler per query type
        IMessageHandler<TQuery, ValueTask<TOutput>> subscriber = GetSubscribers()[0];
        TOutput result = await subscriber.HandleAsync(inputData, ct);
        
        return result;
    }
}
