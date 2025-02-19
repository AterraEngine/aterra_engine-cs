// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum;
using AterraEngine.Frameworks.Continuum.Handlers;

namespace Workfloor.AterraEngine.Frameworks.Continuum.TriggerHandlers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SimpleTriggerHandler : TriggerHandler<SimpleTrigger> {

    public override Task HandleAsync(SimpleTrigger trigger, CancellationToken ct = default) {
        Console.WriteLine($"T1 Received at : {trigger.DateTime}");
        Console.WriteLine($"T1 Processing event: {trigger.Input}");
        
        return Task.CompletedTask;
    }
}
