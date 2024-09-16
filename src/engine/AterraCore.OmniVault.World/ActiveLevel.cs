// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ActiveLevel : IActiveLevel {
    public required INexitiesLevel RawLevelData { get; init; }
    public required ImmutableArray<INexitiesSystem> LogicSystems { get; init; }
    public required ImmutableArray<INexitiesSystem> RenderSystems { get; init; }
    public required ImmutableArray<INexitiesSystem> UiSystems { get; init; }
    public required ImmutableArray<INexitiesSystem> RenderSystemsReversed { get; init; }
    public required IEntityNodeTree ActiveEntityTree { get; init; }
    public required IRaylibCamera2D? Camera2DEntity { get; init; }
    public required FrozenSet<AssetId> TextureAssetIds { get; init; }
}
