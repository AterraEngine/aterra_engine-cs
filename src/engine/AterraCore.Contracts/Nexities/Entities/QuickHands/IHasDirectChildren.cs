﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraCore.Contracts.Nexities.Entities.QuickHands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasDirectChildren : IAssetInstance {
    IDirectChildren ChildrenIDs { get; }
}
