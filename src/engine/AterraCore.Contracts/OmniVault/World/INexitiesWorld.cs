// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAterraCoreWorld {
    /// <summary>
    /// Represents the current active level.
    /// </summary>
    ActiveLevel? ActiveLevel { get; }

    /// <summary> Tries to change the active level in the AterraCoreWorld. </summary>
    /// <param name="levelId">The ID of the level to change to.</param>
    /// <returns>True if the active level was successfully changed, false otherwise.</returns>
    bool TryChangeActiveLevel(AssetId levelId);
    bool TryChangeActiveLevel(INexitiesLevel levelInstance);
    bool TryChangeActiveLevel(AssetId levelId, Ulid levelInstanceId);

    bool TryGetActiveLevel([NotNullWhen(true)] out ActiveLevel? level);
}
