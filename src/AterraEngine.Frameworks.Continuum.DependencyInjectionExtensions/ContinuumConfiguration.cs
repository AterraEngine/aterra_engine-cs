// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;

namespace AterraEngine.Frameworks.Continuum.DependencyInjectionExtensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record ContinuumConfiguration(
    Action<IScopedProvider, IMessageBusFactory> ConfigureMessageBus
) : IMessageBusFactoryConfiguration ;
