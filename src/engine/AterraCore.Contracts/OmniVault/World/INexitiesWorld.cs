// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAterraCoreWorld {
    bool TryChangeActiveLevel(AssetId levelId);
    bool TryGetActiveLevel([NotNullWhen(true)] out IActiveLevel? level);
}
