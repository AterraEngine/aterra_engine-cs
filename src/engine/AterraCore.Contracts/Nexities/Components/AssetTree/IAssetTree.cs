// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;

namespace AterraCore.Contracts.Nexities.Components.AssetTree;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetTree: IComponent {
    public IEnumerable<IAssetInstance> Nodes { get; }

    public IEnumerable<T> OfType<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance;
} 