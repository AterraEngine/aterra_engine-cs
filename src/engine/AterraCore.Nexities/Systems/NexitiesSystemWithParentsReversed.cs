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
    where TChild : class, IAssetInstance {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override ReadOnlySpan<(TParent? Parent, TChild Child, int zIndex)> GetEntities(ActiveLevel level) {
        if (BufferPopulated) return EntitiesBuffer;

        List<(TParent? Parent, TChild Child, int zIndex)> list = ParentChildPool.Get();
        int zIndex = 0;
        foreach ((IAssetInstance? parent, IAssetInstance childInstance) in level.ActiveEntityTree.GetAsFlatReverseWithParent()) {
            int i = zIndex++;
            if (childInstance is TChild child) list.Add((parent as TParent, child, i));
        }

        BufferPopulated = true;
        list.TrimExcess();
        EntitiesBuffer = list.ToArray();

        ParentChildPool.Return(list);
        return EntitiesBuffer;
    }
}
