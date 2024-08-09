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
    public AssetId ActiveLevelId { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryChangeActiveLevel(AssetId levelId) ;
    public bool TryGetActiveLevel([NotNullWhen(true)] out INexitiesLevel? level);
}
