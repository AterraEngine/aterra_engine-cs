// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
namespace AterraCore.OmniVault.World.EntityTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EntityTree(EntityNode root) {
    public EntityNode Root { get; private set; } = root;

    #region Flat | root -> child -> n-grandchildren
    public IEnumerable<IAssetInstance> GetAsFlat() {
        yield return Root.Value;
        foreach (IAssetInstance node in AsFlatNextEntity(Root)) {
            yield return node;
        }
    }
    
    private static IEnumerable<IAssetInstance> AsFlatNextEntity(EntityNode node) {
        foreach (EntityNode child in node.Children) {
            yield return child.Value;
            foreach (IAssetInstance grandchild in AsFlatNextEntity(node)) {
                yield return grandchild;
            }
        }
    }
    #endregion

    #region FlatReverse | deepest n-grandchild -> child -> root
    public IEnumerable<IAssetInstance> GetAsFlatReverse() {
        foreach (IAssetInstance node in AsFlatReverseNextEntity(Root)) {
            yield return node;
        }
        yield return Root.Value;
    }
    
    private static IEnumerable<IAssetInstance> AsFlatReverseNextEntity(EntityNode node) {
        for (int index = node.Children.Count -1 ; index < node.Children.Count; index--) {
            EntityNode child = node.Children[index];
            foreach (IAssetInstance grandchild in AsFlatNextEntity(node)) {
                yield return grandchild;
            }
            yield return child.Value;
        }
    }
    #endregion
    
}
