// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.OmniVault.Assets;

namespace AterraCore.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesComponent : AssetInstance, INexitiesComponent {
    public virtual bool Cleanup() => false;
}
