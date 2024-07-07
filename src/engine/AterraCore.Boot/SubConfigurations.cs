// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.Boot.Nexities;
using AterraCore.Contracts.Boot.OmniVault;
using System.Collections;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record SubConfigurations(
    IFlexiPlugConfiguration FlexiPlug,
    INexitiesConfiguration Nexities,
    IOmniVaultConfiguration OmniVault
) : ISubConfigurations {

    public IEnumerator<IBootConfiguration> GetEnumerator() {
        yield return FlexiPlug;
        yield return Nexities;
        yield return OmniVault;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
