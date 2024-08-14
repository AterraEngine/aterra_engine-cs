// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAterraCoreWorld {
    public AssetId ActiveLevelId { get; }
    public IReadOnlyCollection<INexitiesSystem> LogicSystems { get; }
    public IReadOnlyCollection<INexitiesSystem> RenderSystems { get; }

    public IReadOnlyCollection<IAssetInstance> ActiveEntities { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryChangeActiveLevel(AssetId levelId) ;
    public bool TryGetActiveLevel([NotNullWhen(true)] out INexitiesLevel? level);
}
