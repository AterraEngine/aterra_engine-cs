// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public abstract class NexitiesSystemWithFilter<TEntity> : NexitiesSystem<TEntity>
    where TEntity : IAssetInstance {
    protected abstract Predicate<TEntity> Filter { get; }
    protected override IEnumerable<TEntity> GetEntities(ActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;

        foreach (IAssetInstance instance in level.ActiveEntityTree.GetAsFlat()) {
            if (instance is TEntity assetInstance && Filter(assetInstance))
                EntitiesBuffer.Add(assetInstance);
        }

        BufferPopulated = true;
        return EntitiesBuffer;
    }
}
