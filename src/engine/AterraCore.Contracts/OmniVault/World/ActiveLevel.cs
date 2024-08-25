// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public readonly struct ActiveLevel {
    public INexitiesLevel2D RawLevelData { get; init; }

    public IEnumerable<INexitiesSystem> Logic { init => LogicSystems = [..value]; }
    public IEnumerable<INexitiesSystem> Render { init => RenderSystems = [..value]; }
    public IEnumerable<INexitiesSystem> Ui { init => UiSystems = [..value]; }

    public ImmutableArray<INexitiesSystem> LogicSystems { get; private init; }
    public ImmutableArray<INexitiesSystem> RenderSystems { get; private init; }
    public ImmutableArray<INexitiesSystem> UiSystems { get; private init; }

    public IEntityNodeTree ActiveEntityTree { get; init; }
    public IRaylibCamera2D? Camera2DEntity { get; init; }

    public IEnumerable<AssetId> TextureAssetIds { get; init; }
}
