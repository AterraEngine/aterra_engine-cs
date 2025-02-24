// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Continuum;

namespace Workfloor.AterraEngine.Frameworks.Continuum.QueryHandlers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SimpleQueryHandler : QueryHandler<SimpleQuery, bool> {

    public override ValueTask<bool> HandleAsync(SimpleQuery input, CancellationToken ct = default) => new(input.Input == "true");
}
