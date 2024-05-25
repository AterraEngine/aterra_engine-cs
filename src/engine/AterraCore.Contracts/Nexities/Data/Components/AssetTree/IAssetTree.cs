﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Contracts.Nexities.Data.Components.AssetTree;

using Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetTree : IComponent {
    public IEnumerable<IAssetInstance> Nodes { get; }

    public IEnumerable<T> OfType<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance;
}