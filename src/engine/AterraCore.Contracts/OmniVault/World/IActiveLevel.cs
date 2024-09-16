// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActiveLevel {
    INexitiesLevel RawLevelData { get; init; }
    
    ImmutableArray<INexitiesSystem> LogicSystems { get; init; }
    ImmutableArray<INexitiesSystem> RenderSystems { get; init; }
    ImmutableArray<INexitiesSystem> UiSystems { get; init; }
    
    ImmutableArray<INexitiesSystem> RenderSystemsReversed { get; init; }

    IEntityNodeTree ActiveEntityTree { get; init; }
    IRaylibCamera2D? Camera2DEntity { get; init; }

    FrozenSet<AssetId> TextureAssetIds { get; init; }
}
