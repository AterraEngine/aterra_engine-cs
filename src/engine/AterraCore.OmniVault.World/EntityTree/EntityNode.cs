﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;

namespace AterraCore.OmniVault.World.EntityTree;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct EntityNode(IAssetInstance asset) : IEntityNode {
    public IAssetInstance Value { get; set; } = asset;
    public List<IEntityNode> Children { get;} = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddChild(IAssetInstance child) {
        Children.Add(new EntityNode(child));
    }
    public void AddChild(IEntityNode node) {
        Children.Add(node);
    }
}
