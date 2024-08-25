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
    ActiveLevel? ActiveLevel { get; }

    bool TryChangeActiveLevel(AssetId levelId);
    bool TryChangeActiveLevel(INexitiesLevel2D levelInstance);
    bool TryChangeActiveLevel(AssetId levelId, Ulid levelInstanceId);

    bool TryGetActiveLevel([NotNullWhen(true)] out ActiveLevel? level);
}
