// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;

namespace AterraEngine.EntityTree;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct EntityNode(IAssetInstance asset) : IEntityNode {
    public IAssetInstance Value { get; } = asset;
    public List<IEntityNode> Children { get; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddChild(IAssetInstance child) {
        Children.Add(new EntityNode(child));
    }
}
