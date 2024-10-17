// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.World.EntityTree;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEntityNodeTree {
    IEnumerable<IAssetInstance> GetAsFlat();
    IEnumerable<IAssetInstance> GetAsFlatReverse();
    IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatWithParent();
    IEnumerable<(IAssetInstance? Parent, IAssetInstance Child)> GetAsFlatReverseWithParent();

    IEnumerable<IEntityNode> TraverseDepthFirst();
    IEnumerable<IEntityNode> TraverseBreadthFirst();

    bool TryGetFirst<T>(Func<IEntityNode, bool> predicate, [NotNullWhen(true)] out T? value) where T : IAssetInstance;
    void FindAndUpdateNodes<T>(Func<IEntityNode, bool> predicate, Action<IEntityNode> action);
}
