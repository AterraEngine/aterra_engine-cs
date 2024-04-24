// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.Contracts.DI;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate void StaticService(IServiceCollection serviceCollection, ILogger logger);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngineServiceBuilder {
    public IServiceCollection ServiceCollection { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AssignDefaultServices(IEnumerable<StaticService> services);
    public void AssignStaticServices(IEnumerable<StaticService> services);
    public void FinishBuilding();
}