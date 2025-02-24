// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITriggerHubBuilder : IMessageHubBuilder<ITriggerHub>;

public interface ITriggerHubBuilder<out TTrigger> : ITriggerHubBuilder
    where TTrigger : ITrigger {
    
    ITriggerHubBuilder<TTrigger> WithHandler<TTriggerHandler>() where TTriggerHandler : class, ITriggerHandler<TTrigger>;
    ITriggerHubBuilder<TTrigger> WithPipelineSteps(params Type[] types);
}
