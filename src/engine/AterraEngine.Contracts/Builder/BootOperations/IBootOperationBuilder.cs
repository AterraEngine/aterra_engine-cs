// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Contracts.Builder.BootOperations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootOperationBuilder {
    Task BuildAsync(IServiceCollection serviceCollection, CancellationToken cancellationToken = default);
}
