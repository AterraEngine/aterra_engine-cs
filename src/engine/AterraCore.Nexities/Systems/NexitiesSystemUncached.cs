// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.OmniVault.Assets;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesSystemUnCached<TEntity> : AssetInstance, INexitiesSystem 
    where TEntity : IAssetInstance
{
    protected virtual Predicate<TEntity> Filter { get; } = _ => true;
    private static bool EntitiesReversed => false;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(IActiveLevel level);
    public virtual void ClearCaches() {}

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected IEnumerable<TEntity> GetEntities(IActiveLevel level) {
        IEnumerable<IAssetInstance> entities = EntitiesReversed 
            ? level.ActiveEntityTree.GetAsFlatReverse() 
            : level.ActiveEntityTree.GetAsFlat();
        
        foreach (IAssetInstance instance in entities) {
            if (instance is TEntity assetInstance && Filter(assetInstance)) 
                yield return assetInstance;
        }
    }
}
