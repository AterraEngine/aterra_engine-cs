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
public abstract class NexitiesSystemWithParents<TParent, TChild> : AssetInstance, INexitiesSystem
    where TParent : class, IAssetInstance 
    where TChild : class, IAssetInstance
{
    protected bool BufferPopulated;
    protected (TParent? Parent,TChild Child)[] EntitiesBuffer = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(ActiveLevel level);
    public virtual void InvalidateCaches() {
        EntitiesBuffer = [];
        BufferPopulated = false;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected virtual (TParent? Parent,TChild Child)[] GetEntities(ActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;

        var list = new List<(TParent? Parent, TChild Child)>();
        foreach ((IAssetInstance? Parent, IAssetInstance Child) instance in level.ActiveEntityTree.GetAsFlatWithParent()) {
            var parent = instance.Parent as TParent;
            if (instance.Child is TChild child) 
                list.Add((parent, child));
        }
        
        BufferPopulated = true;
        return EntitiesBuffer = list.ToArray();
    }
    
}