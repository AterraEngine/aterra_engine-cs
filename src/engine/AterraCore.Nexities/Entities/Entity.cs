﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public abstract class Entity(params IComponent?[]? components) : AssetInstance, IComponent {
    public HashSet<Guid> ComponentIds { get; } =
        components?
            .Where(comp => comp != null)
            .Select(comp => comp!.Guid)
            .ToHashSet() ?? [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IComponent[] GetComponents() {
        throw new NotImplementedException();
    }
}