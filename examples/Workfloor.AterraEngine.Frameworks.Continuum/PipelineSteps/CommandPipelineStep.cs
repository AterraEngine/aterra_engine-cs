// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum;
using AterraEngine.Frameworks.Continuum.PipelineSteps;

namespace Workfloor.AterraEngine.Frameworks.Continuum.PipelineSteps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SimpleCommandPipelineStep<TCommand, TResult> : CommandPipelineStep<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : struct
{
    public async override ValueTask<TResult> HandleAsync(TCommand command, CancellationToken ct = default) {
        // Do stuff before next step
        Console.WriteLine("Doing stuff before next step");
        
        // Do next step
        TResult result = await NextStep(command, ct);

        // Do stuff after next step
        Console.WriteLine("Doing stuff after next step");

        return result;
    }    
}
