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
public abstract class NexitiesSystemWithParents<TParent, TChild> : AssetInstance, INexitiesSystem
    where TParent : class, IAssetInstance 
    where TChild : class, IAssetInstance
{
    private readonly List<(TParent? Parent,TChild Child)> _entitiesBuffer = [];
    protected virtual Predicate<(TParent? Parent,TChild Child)> Filter { get; } = _ => true;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(IActiveLevel level);

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected IEnumerable<(TParent? Parent,TChild Child)> GetEntities(IActiveLevel level) {
        if (_entitiesBuffer.Count != 0) return _entitiesBuffer;

        IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> entities = level.ActiveEntityTree.GetAsFlatWithParent();
        
        _entitiesBuffer.Clear(); // Reuse the buffer instead of allocating a new one
        
        foreach ((IAssetInstance? Parent, IAssetInstance Child) instance in entities) {
            var parent = instance.Parent as TParent;
            if (instance.Child is TChild child && Filter((parent, child))) 
                _entitiesBuffer.Add((parent, child));
        }
        
        return _entitiesBuffer;
    }
    
}