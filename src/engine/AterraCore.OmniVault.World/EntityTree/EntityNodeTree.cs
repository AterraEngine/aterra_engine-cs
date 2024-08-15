// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;

namespace AterraCore.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct EntityNodeTree(IEntityNode root) : IEntityNodeTree {
    private IEntityNode Root { get; } = root;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------=
    #region Flat | root -> child -> n-grandchildren
    public IEnumerable<IAssetInstance> GetAsFlat() {
        if (Root.Value != null) yield return Root.Value;
        foreach (IAssetInstance node in AsFlatNextEntity(Root))
            yield return node;
    }
    
    private static IEnumerable<IAssetInstance> AsFlatNextEntity(IEntityNode node) {
        foreach (IEntityNode child in node.Children) {
            if (child.Value != null) yield return child.Value;
            foreach (IAssetInstance grandchild in AsFlatNextEntity(node))
                yield return grandchild;
        }
    }
    #endregion

    #region FlatReverse | deepest n-grandchild -> child -> root
    public IEnumerable<IAssetInstance> GetAsFlatReverse() {
        foreach (IAssetInstance node in AsFlatReverseNextEntity(Root))
            yield return node;
        if (Root.Value != null) yield return Root.Value;
    }
    
    private static IEnumerable<IAssetInstance> AsFlatReverseNextEntity(IEntityNode node) {
        for (int index = node.Children.Count -1 ; index < node.Children.Count; index--) {
            IEntityNode child = node.Children[index];
            foreach (IAssetInstance grandchild in AsFlatNextEntity(node))
                yield return grandchild;
            if (child.Value != null) yield return child.Value;
        }
    }
    #endregion
    
}
