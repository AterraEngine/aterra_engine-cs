// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;

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
    protected (TParent? Parent, TChild Child, int zIndex)[] EntitiesBuffer = [];

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
    protected virtual (TParent? Parent, TChild Child, int zIndex)[] GetEntities(ActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;

        List<(TParent? Parent, TChild Child, int zIndex)> list = ParentChildPool.Get();
        int zIndex = 0;
        foreach ((IAssetInstance? parent, IAssetInstance childInstance) in level.ActiveEntityTree.GetAsFlatWithParent()) {
            int i = zIndex++;
            if (childInstance is TChild child) list.Add((parent as TParent, child, i));
        }

        BufferPopulated = true;
        list.TrimExcess();
        EntitiesBuffer = list.ToArray();

        ParentChildPool.Return(list);
        return EntitiesBuffer;
    }

    #region Reusable Pool
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();
    private ObjectPool<List<(TParent? Parent, TChild Child, int zIndex)>>? _parentChildPool;
    protected ObjectPool<List<(TParent? Parent, TChild Child, int zIndex)>> ParentChildPool =>
        _parentChildPool ??= _objectPoolProvider.Create(new ComponentsByIdPoolPolicy(1024 * 64));

    private class ComponentsByIdPoolPolicy(int capacity) : PooledObjectPolicy<List<(TParent? Parent, TChild Child, int zIndex)>> {
        public override List<(TParent? Parent, TChild Child, int zIndex)> Create() => new(capacity);
        public override bool Return(List<(TParent? Parent, TChild Child, int zIndex)> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
}
