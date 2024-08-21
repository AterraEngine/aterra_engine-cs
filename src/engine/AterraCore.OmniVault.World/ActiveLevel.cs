// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;

namespace AterraCore.OmniVault.World;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct ActiveLevel : IActiveLevel {
    public INexitiesLevel2D RawLevelData { get; init; }
    public IReadOnlyCollection<INexitiesSystem> LogicSystems { get; init; }
    public IReadOnlyCollection<INexitiesSystem> RenderSystems { get; init; }
    public IEntityNodeTree ActiveEntityTree { get; init; }
    public IRaylibCamera2D? Camera2DEntity { get; init; }

    public IEnumerable<AssetId> TextureAssetIds { get; init; }
}
