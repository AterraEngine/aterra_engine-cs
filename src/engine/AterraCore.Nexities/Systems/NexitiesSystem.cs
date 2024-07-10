// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Systems;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesSystem<TEntity> : INexitiesSystem where TEntity : IAssetInstance {
    public Type ProcessableEntityType { get; } = typeof(TEntity);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool IsRequiredType(IAssetInstance instance) => instance is TEntity;// Use this for caching <-- ??? how?
    public void ProcessLevel(INexitiesLevel level) {
        foreach (TEntity instance in level.ChildEntities.OfTypeManyReverse<TEntity>()) {
            ProcessEntity(instance);
        }
    }

    protected abstract void ProcessEntity(TEntity entity);
}
