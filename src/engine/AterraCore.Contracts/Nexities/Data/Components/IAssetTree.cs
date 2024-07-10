// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Assets;

namespace AterraCore.Contracts.Nexities.Data.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetTree : INexitiesComponent {
    public LinkedList<IAssetInstance> Nodes { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<T> OfType<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeReverse<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeMany<T>() where T : IAssetInstance;
    public IEnumerable<T> OfTypeManyReverse<T>() where T : IAssetInstance;
}
