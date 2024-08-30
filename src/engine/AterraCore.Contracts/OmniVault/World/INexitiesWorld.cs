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
    
    bool TryChangeActiveLevel(INexitiesLevel levelInstance);
    bool TryChangeActiveLevel(AssetId levelId, Ulid? levelInstanceId = null);

    bool TryGetActiveLevel([NotNullWhen(true)] out ActiveLevel? level);
}
