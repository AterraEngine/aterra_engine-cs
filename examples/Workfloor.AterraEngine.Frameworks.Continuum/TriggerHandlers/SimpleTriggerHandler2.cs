// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum;

namespace Workfloor.AterraEngine.Frameworks.Continuum.TriggerHandlers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SimpleTriggerHandler2 : ITriggerHandler<SimpleTrigger> {

    public Task HandleAsync(SimpleTrigger trigger, CancellationToken ct = default) {
        Console.WriteLine($"T2 Received at : {trigger.DateTime}");
        Console.WriteLine($"T2 Processing event: {trigger.Input}");
        
        return Task.CompletedTask;
    }
}
