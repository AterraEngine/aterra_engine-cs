// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.Boot.Nexities;
using AterraCore.Contracts.Boot.OmniVault;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ISubConfigurations : IEnumerable<IBootConfiguration> {
    public IFlexiPlugConfiguration FlexiPlug { get; init; }
    public INexitiesConfiguration Nexities { get; init; }
    public IOmniVaultConfiguration OmniVault { get; init; }
}
