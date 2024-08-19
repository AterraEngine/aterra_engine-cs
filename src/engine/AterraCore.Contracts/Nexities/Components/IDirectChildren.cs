﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDirectChildren : INexitiesComponent {
    int Count { get; }
    IReadOnlyCollection<Ulid> Children { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryAddFirst(Ulid id);
    bool TryAdd(Ulid id);
    bool TryInsertBefore(Ulid id, Ulid before);
    bool TryInsertAfter(Ulid id, Ulid after);
}