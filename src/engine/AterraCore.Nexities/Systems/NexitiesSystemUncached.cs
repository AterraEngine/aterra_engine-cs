// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.OmniVault.Assets;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public abstract class NexitiesSystemUnCached<TEntity> : AssetInstance, INexitiesSystem 
    where TEntity : IAssetInstance
{
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(ActiveLevel level);
    public virtual void InvalidateCaches() {}

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected IEnumerable<TEntity> GetEntities(ActiveLevel level) {
        foreach (IAssetInstance instance in level.ActiveEntityTree.GetAsFlat()) {
            if (instance is TEntity assetInstance) 
                yield return assetInstance;
        }
    }
}
