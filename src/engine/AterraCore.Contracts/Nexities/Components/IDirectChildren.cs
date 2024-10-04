// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDirectChildren : INexitiesComponent {
    int Count { get; }
    int CountNested { get; }
    IReadOnlyCollection<Ulid> Children { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryAddFirst(Ulid id);
    bool TryAdd(Ulid id);
    bool TryAdd<T>(T asset) where T : IAssetInstance;
    
    bool TryInsertBefore(Ulid id, Ulid before);
    bool TryInsertAfter(Ulid id, Ulid after);
}
