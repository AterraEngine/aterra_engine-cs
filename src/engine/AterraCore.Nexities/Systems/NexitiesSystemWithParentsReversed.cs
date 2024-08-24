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
    protected override (TParent? Parent,TChild Child)[] GetEntities(ActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;
        
        var list = new List<(TParent? Parent, TChild Child)>();
        foreach ((IAssetInstance? Parent, IAssetInstance Child) instance in level.ActiveEntityTree.GetAsFlatReverseWithParent())
            if (instance.Child is TChild child)
                list.Add((instance.Parent as TParent, child));
        
        BufferPopulated = true;
        return EntitiesBuffer = list.ToArray();
    }
    
}