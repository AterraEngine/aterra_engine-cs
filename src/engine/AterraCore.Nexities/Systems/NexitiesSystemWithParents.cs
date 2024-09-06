// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.OmniVault.Assets;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public abstract class NexitiesSystemWithParents<TParent, TChild> : AssetInstance, INexitiesSystem
    where TParent : class, IAssetInstance
    where TChild : class, IAssetInstance {
    protected bool BufferPopulated;
    protected (TParent? Parent, TChild Child)[] EntitiesBuffer = [];
    
    #region Reusable Pool
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();
    private ObjectPool<List<(TParent? Parent, TChild Child)>>? _parentChildPool;
    protected ObjectPool<List<(TParent? Parent, TChild Child)>> ParentChildPool =>
        _parentChildPool ??= _objectPoolProvider.Create(new ComponentsByIdPoolPolicy(1024*64));

    private class ComponentsByIdPoolPolicy(int capacity) : PooledObjectPolicy<List<(TParent? Parent, TChild Child)>> {
        public override List<(TParent? Parent, TChild Child)> Create() => new(capacity);
        public override bool Return(List<(TParent? Parent, TChild Child)> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion

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
    protected virtual (TParent? Parent, TChild Child)[] GetEntities(ActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;

        List<(TParent? Parent, TChild Child)> list = ParentChildPool.Get();
        foreach ((IAssetInstance? Parent, IAssetInstance Child) instance in level.ActiveEntityTree.GetAsFlatWithParent()) {
            var parent = instance.Parent as TParent;
            if (instance.Child is TChild child)
                list.Add((parent, child));
        }

        BufferPopulated = true;
        list.TrimExcess();
        EntitiesBuffer = list.ToArray();
        
        ParentChildPool.Return(list);
        return EntitiesBuffer;
    }
}
