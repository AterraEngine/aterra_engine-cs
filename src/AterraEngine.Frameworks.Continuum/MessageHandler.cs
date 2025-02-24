// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class MessageHandler<TInput, TOutput> : IMessageHandler<TInput, TOutput> {
    public Guid Id { get; } = Guid.CreateVersion7();
    public abstract TOutput HandleAsync(TInput input, CancellationToken ct = default);
}
