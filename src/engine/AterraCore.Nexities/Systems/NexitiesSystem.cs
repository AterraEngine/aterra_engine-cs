// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Levels;

namespace AterraCore.Nexities.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesSystem<TEntity> where TEntity : IAssetInstance {
    public Type ProcessableEntityType = typeof(TEntity);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool IsRequiredType(IAssetInstance instance) => instance is TEntity; // Use this for caching <-- ??? how?

    public void ProcessEntities(ILevel level) {
        foreach (TEntity instance in level.ChildEntities.OfTypeManyReverse<TEntity>()) {
            ProcessEntity(instance);
        }
    }
    
    protected abstract void ProcessEntity(TEntity entity);
}