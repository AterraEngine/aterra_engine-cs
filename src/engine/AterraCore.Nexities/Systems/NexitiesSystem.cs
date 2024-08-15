// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.OmniVault.Assets;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesSystem<TEntity>(bool entitiesReversed = false, bool uncached=false) : AssetInstance, INexitiesSystem where TEntity : IAssetInstance {
    private TEntity[]? _entitiesBuffer;
    protected virtual Predicate<TEntity> Filter { get; } = _ => true;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(IAterraCoreWorld world);

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected TEntity[] GetEntities(IAterraCoreWorld world) {
        if (!uncached  && _entitiesBuffer is not null) return _entitiesBuffer;
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return [];
        
        IEnumerable<IAssetInstance> entities = entitiesReversed 
            ? level.ActiveEntityTree.GetAsFlatReverse() 
            : level.ActiveEntityTree.GetAsFlat();
        
        var list = new List<TEntity>();
        foreach (IAssetInstance instance in entities) {
            if (instance is TEntity assetInstance && Filter(assetInstance)) list.Add(assetInstance);
        }
        
        _entitiesBuffer = list.ToArray();
        return _entitiesBuffer;
    }
}
