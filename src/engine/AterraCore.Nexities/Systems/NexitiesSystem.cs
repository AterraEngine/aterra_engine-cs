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
public abstract class NexitiesSystem<TEntity> : AssetInstance, INexitiesSystem
    where TEntity : IAssetInstance {
    protected readonly List<TEntity> EntitiesBuffer = [];
    protected bool BufferPopulated;

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected virtual IEnumerable<TEntity> GetEntities(IActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;

        foreach (IAssetInstance instance in level.ActiveEntityTree.GetAsFlat()) {
            if (instance is TEntity assetInstance)
                EntitiesBuffer.Add(assetInstance);
        }

        BufferPopulated = true;
        return EntitiesBuffer;
    }
}

[UsedImplicitly]
public abstract class NexitiesSystem : NexitiesSystem<IAssetInstance>;
