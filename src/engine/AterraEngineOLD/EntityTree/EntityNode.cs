// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;

namespace AterraEngineOLD.EntityTree;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct EntityNode(IAssetInstance asset) : IEntityNode {
    public IAssetInstance Value { get; } = asset;
    public List<IEntityNode> Children { get; } = [];


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddChild<T>(T child) where T : IAssetInstance {
        Children.Add(new EntityNode(child));
    }
    public void UpdateAndCascade() {
        throw new NotImplementedException();
    }
}
