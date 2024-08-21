﻿// ---------------------------------------------------------------------------------------------------------------------
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
    protected readonly List<(TParent? Parent,TChild Child)> EntitiesBuffer = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void Tick(IActiveLevel level);
    public virtual void InvalidateCaches() {
        EntitiesBuffer.Clear();
        BufferPopulated = false;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected virtual IEnumerable<(TParent? Parent,TChild Child)> GetEntities(IActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;

        foreach ((IAssetInstance? Parent, IAssetInstance Child) instance in level.ActiveEntityTree.GetAsFlatWithParent()) {
            var parent = instance.Parent as TParent;
            if (instance.Child is TChild child) 
                EntitiesBuffer.Add((parent, child));
        }
        
        BufferPopulated = true;
        return EntitiesBuffer;
    }
    
}