// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.DI;
using AterraCore.OmniVault.Assets;
using Serilog;
using Serilog.Core;

namespace AterraCore.Nexities.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesSystemWithParentsReversed<TParent, TChild> : AssetInstance, INexitiesSystem
    where TParent : class, IAssetInstance 
    where TChild : class, IAssetInstance
{
    private (TParent? Parent,TChild Child)[] _entitiesBuffer = [];
    protected virtual Predicate<(TParent? Parent,TChild Child)> Filter { get; } = _ => true;
    private ILogger _logger = EngineServices.GetLogger();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(IActiveLevel level);

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected (TParent? Parent,TChild Child)[] GetEntities(IActiveLevel level) {
        if (_entitiesBuffer.Length != 0) return _entitiesBuffer;
        
        IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> entities = level.ActiveEntityTree.GetAsFlatReverseWithParent();
        
        // _entitiesBuffer.Clear(); // Reuse the buffer instead of allocating a new one

        // ReSharper disable once SuggestVarOrType_Elsewhere
        var valueTuples = entities.ToArray();
        List<(TParent? Parent,TChild Child)> temp = new(valueTuples.Length);
        foreach ((IAssetInstance? Parent, IAssetInstance Child) instance in valueTuples) {
            var parent = instance.Parent as TParent;
            if (instance.Child is TChild child && Filter((parent, child))) 
                temp.Add((parent, child));
        }
        temp.TrimExcess();
        _entitiesBuffer = temp.ToArray();
        _logger.Debug($"Entities buffer length: {_entitiesBuffer.Length}");
        return _entitiesBuffer;
    }
    
}