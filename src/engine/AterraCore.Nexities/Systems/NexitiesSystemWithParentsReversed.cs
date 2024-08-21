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
public abstract class NexitiesSystemWithParentsReversed<TParent, TChild> : NexitiesSystemWithParents<TParent, TChild>
    where TParent : class, IAssetInstance 
    where TChild : class, IAssetInstance
{
    protected override IEnumerable<(TParent? Parent,TChild Child)> GetEntities(IActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;
        
        foreach ((IAssetInstance? Parent, IAssetInstance Child) instance in level.ActiveEntityTree.GetAsFlatReverseWithParent()) {
            var parent = instance.Parent as TParent;
            if (instance.Child is TChild child) 
                EntitiesBuffer.Add((parent, child));
        }
        BufferPopulated = true;
        return EntitiesBuffer;
    }
    
}