// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Components;

namespace AterraCore.Contracts.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEntity : IAsset {
    public HashSet<Guid> ComponentIds { get; }
    public IComponent[] GetComponents();
}