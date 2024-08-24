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
public abstract class NexitiesSystemUnCachedWithFilter<TEntity> : AssetInstance, INexitiesSystem 
    where TEntity : IAssetInstance
{
    protected abstract Predicate<TEntity> Filter { get; }
    
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
            if (instance is TEntity assetInstance && Filter(assetInstance)) 
                yield return assetInstance;
        }
    }
}
