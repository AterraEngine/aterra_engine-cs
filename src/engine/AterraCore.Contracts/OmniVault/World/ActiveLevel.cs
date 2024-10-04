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
public class ActiveLevel(IEntityTreeFactory entityTreeFactory) {
    public required INexitiesLevel RawLevelData { get; init; }
    public required ImmutableArray<ILogicSytem> LogicSystems { get; init; }
    public required ImmutableArray<IRenderSystem> RenderSystems { get; init; }
    public required ImmutableArray<IUiSystem> UiSystems { get; init; }
    public required ImmutableArray<IRenderSystem> RenderSystemsReversed { get; init; }
    
    private IEntityNodeTree? _activeEntityTree;
    public required IEntityNodeTree ActiveEntityTree {
        get => _activeEntityTree ??= entityTreeFactory.CreateFromRootId(RawLevelData.InstanceId);
        init => _activeEntityTree = value;
    }
    public required IRaylibCamera2D? Camera2DEntity { get; init; }
    public required FrozenSet<AssetId> TextureAssetIds { get; init; }
    
    public void ResetActiveEntityTree() => _activeEntityTree = null;
}
