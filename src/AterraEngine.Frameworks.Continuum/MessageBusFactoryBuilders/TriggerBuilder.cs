// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Continuum;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TriggerBuilder<TTrigger>(ITriggerHub<TTrigger> hub, IScopedProvider provider) : ITriggerBuilder<TTrigger>
    where TTrigger : ITrigger 
{
    public ITriggerBuilder<TTrigger> WithHandler<TTriggerHandler>() where TTriggerHandler : class, ITriggerHandler<TTrigger> {
        var handler = provider.GetRequiredService<TTriggerHandler>(); 
        hub.Subscribe(handler);
        return this;
    }
}
