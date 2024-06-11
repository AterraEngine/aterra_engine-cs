// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.Boot.Nexities;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public record SubConfigurations(
    IFlexiPlugConfiguration FlexiPlug,
    INexitiesConfiguration Nexities
) : ISubConfigurations;
