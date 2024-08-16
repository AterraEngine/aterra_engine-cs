// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.DI;
using AterraCore.OmniVault.Assets;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using Serilog.Core;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesSystem<TEntity> : AssetInstance, INexitiesSystem 
    where TEntity : IAssetInstance
{
    private List<TEntity>? _entitiesBuffer;
    protected virtual Predicate<TEntity> Filter { get; } = _ => true;
    protected virtual bool EntitiesReversed => false;
    protected virtual bool Uncached => false;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(IAterraCoreWorld world);

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected IEnumerable<TEntity> GetEntities(IAterraCoreWorld world) {
        if (!Uncached && _entitiesBuffer?.Count > 0) return _entitiesBuffer;
        if (!world.TryGetActiveLevel(out IActiveLevel? level)) return [];
        
        IEnumerable<IAssetInstance> entities = EntitiesReversed 
            ? level.ActiveEntityTree.GetAsFlatReverse() 
            : level.ActiveEntityTree.GetAsFlat();
        
        _entitiesBuffer ??= [];
        _entitiesBuffer.Clear(); // Reuse the buffer instead of allocating a new one
        
        foreach (IAssetInstance instance in entities) {
            if (instance is TEntity assetInstance && Filter(assetInstance)) 
                _entitiesBuffer.Add(assetInstance);
        }
        
        return _entitiesBuffer;
    }
}
