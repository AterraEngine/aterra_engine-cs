// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TriggerBuilder<TTrigger> : ITriggerBuilder<TTrigger>
    where TTrigger : ITrigger {

    public Type TriggerType { get; } = typeof(TTrigger);
    public Type? TriggerHandlerType { get; private set; }
    public Type[] PipelineSteps { get; private set; } = [];


    public ITriggerBuilder<TTrigger> WithHandler<TTriggerHandler>() where TTriggerHandler : class, ITriggerHandler<TTrigger> {
        TriggerHandlerType = typeof(TTriggerHandler);
        return this;
    }
    
    public ITriggerBuilder<TTrigger> WithPipelineSteps(params Type[] types) {
        PipelineSteps = PipelineSteps.Concat(types).ToArray();
        return this;
    }
}
