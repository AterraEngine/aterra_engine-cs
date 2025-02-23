// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITriggerBuilder{
    Type TriggerType { get; }
}

public interface ITriggerBuilder<out TTrigger> : ITriggerBuilder
    where TTrigger : ITrigger {
    
    ITriggerBuilder<TTrigger> WithHandler<TTriggerHandler>() where TTriggerHandler : class, ITriggerHandler<TTrigger>;
    ITriggerBuilder<TTrigger> WithPipelineSteps(params Type[] types);
}
