// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum;
using AterraEngine.Frameworks.Continuum.Handlers;

namespace Workfloor.AterraEngine.Frameworks.Continuum.CommandHandlers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SimpleCommandHandler : CommandHandler<SimpleCommand, bool> {
    public async override ValueTask<bool> HandleAsync(SimpleCommand command, CancellationToken ct = default) {
        Console.WriteLine($"Received at : {command.DateTime}");
        
        await Task.Delay(TimeSpan.FromSeconds(1), ct); // Wait 1 second
        Console.WriteLine($"Processing command: {command.Input}");
        return command.Input == "true";
    }
}
