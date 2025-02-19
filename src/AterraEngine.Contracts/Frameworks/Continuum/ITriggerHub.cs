// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITriggerHub {
    bool HasSubscriptions { get; }
    
    ValueTask PublishAsync<T>(T eventData, CancellationToken ct = default) where T : ITrigger;
    Task StartProcessingAsync();
}

public interface ITriggerHub<TTrigger> : ITriggerHub where TTrigger : ITrigger {
    void Subscribe<TTriggerHandler>(TTriggerHandler handler) where TTriggerHandler : ITriggerHandler<TTrigger>;
}