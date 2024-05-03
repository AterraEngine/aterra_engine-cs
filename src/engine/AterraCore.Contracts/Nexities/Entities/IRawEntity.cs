// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Components;

namespace AterraCore.Contracts.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IRawEntity<out T> : IAssetInstance where T : IComponent {
    public HashSet<Guid> ComponentIds { get; }
    public IEnumerable<T> GetComponents();
}