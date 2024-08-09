// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesSystem<TEntity> : INexitiesSystem where TEntity : IAssetInstance {
    public Type ProcessableEntityType { get; } = typeof(TEntity);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ProcessLevel(INexitiesLevel? level) {
        foreach (TEntity instance in SelectEntities(level)) {
            ProcessEntity(instance);
        }
    }

    protected abstract IEnumerable<TEntity> SelectEntities(INexitiesLevel? level);
    protected abstract void ProcessEntity(TEntity entity);
}
