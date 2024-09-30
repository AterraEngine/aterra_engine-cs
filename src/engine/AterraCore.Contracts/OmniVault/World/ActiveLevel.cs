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
using System.Runtime.InteropServices;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public readonly struct ActiveLevel {
    public INexitiesLevel RawLevelData { get; init; }

    public ImmutableArray<ILogicSytem> LogicSystems { get; init; }
    public ImmutableArray<IRenderSystem> RenderSystems { get; init; }
    public ImmutableArray<IUiSystem> UiSystems { get; init; }

    public ImmutableArray<IRenderSystem> RenderSystemsReversed { get; init; }

    public IEntityNodeTree ActiveEntityTree { get; init; }
    public IRaylibCamera2D? Camera2DEntity { get; init; }

    public FrozenSet<AssetId> TextureAssetIds { get; init; }
}
