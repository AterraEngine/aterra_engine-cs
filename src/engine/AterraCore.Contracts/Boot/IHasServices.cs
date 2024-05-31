// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IHasServices {
    public IEnumerable<ServiceDescriptor> DefineDefaultServices();
    public IEnumerable<ServiceDescriptor> DefineStaticServices();
}
