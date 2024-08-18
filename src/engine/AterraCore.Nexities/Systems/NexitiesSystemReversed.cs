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
public abstract class NexitiesSystemReversed<TEntity> : AssetInstance, INexitiesSystem 
    where TEntity : IAssetInstance
{
    private readonly List<TEntity> _entitiesBuffer = [];
    protected virtual Predicate<TEntity> Filter { get; } = _ => true;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(IActiveLevel level);

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected IEnumerable<TEntity> GetEntities(IActiveLevel level) {
        if (_entitiesBuffer.Count != 0) return _entitiesBuffer;

        _entitiesBuffer.Clear(); // Reuse the buffer instead of allocating a new one
        
        foreach (IAssetInstance instance in level.ActiveEntityTree.GetAsFlatReverse()) {
            if (instance is TEntity assetInstance && Filter(assetInstance)) 
                _entitiesBuffer.Add(assetInstance);
        }
        
        return _entitiesBuffer;
    }
}
