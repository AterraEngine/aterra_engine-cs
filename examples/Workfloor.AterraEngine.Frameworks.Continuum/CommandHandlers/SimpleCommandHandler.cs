// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum;

namespace Workfloor.AterraEngine.Frameworks.Continuum.CommandHandlers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SimpleCommandHandler : CommandHandler<SimpleCommand, bool> {
    public override ValueTask<bool> HandleAsync(SimpleCommand command) {
        if (command.input == "true") return new ValueTask<bool>(true);
        return new ValueTask<bool>(false);
    }
}
