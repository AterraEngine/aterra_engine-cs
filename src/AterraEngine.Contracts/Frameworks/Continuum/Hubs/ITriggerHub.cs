// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum.Handlers;

namespace AterraEngine.Frameworks.Continuum.Hubs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITriggerHub {
    Task StartProcessingAsync();
}

public interface ITriggerHub<TTrigger> : ITriggerHub, IMessageHub<ITriggerHandler<TTrigger>, TTrigger, Task> 
    where TTrigger : ITrigger ;