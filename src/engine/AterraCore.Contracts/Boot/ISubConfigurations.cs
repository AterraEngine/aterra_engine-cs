// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.Boot.Nexities;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ISubConfigurations {
    public IFlexiPlugConfiguration FlexiPlug { get; init; }
    public INexitiesConfiguration Nexities { get; init; }
}
