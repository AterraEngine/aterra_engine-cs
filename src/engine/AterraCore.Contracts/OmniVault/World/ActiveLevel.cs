// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ActiveLevel(IEntityTreeFactory entityTreeFactory, IAssetAtlas assetAtlas) {
    public required INexitiesLevel RawLevelData { get; init; }
    public required ImmutableArray<ILogicSytem> LogicSystems { get; init; }
    public required ImmutableArray<IRenderSystem> RenderSystems { get; init; }
    public required ImmutableArray<IUiSystem> UiSystems { get; init; }
    public required ImmutableArray<IRenderSystem> RenderSystemsReversed { get; init; }
    
    private IEntityNodeTree? _activeEntityTree;
    public required IEntityNodeTree ActiveEntityTree {
        get => _activeEntityTree ??= entityTreeFactory.CreateFromRootId(RawLevelData.InstanceId);
        set => _activeEntityTree = value;
    }
    public required IRaylibCamera2D? Camera2DEntity { get; init; }
    
    private ConcurrentBag<AssetId>? _textureAssetIds;
    public required ConcurrentBag<AssetId> TextureAssetIds {
        get => _textureAssetIds ??= new ConcurrentBag<AssetId>(ActiveEntityTree.GetAsFlat()
            .Where(asset => asset is IIsRenderable2D)
            .Select(asset => ((IIsRenderable2D)asset).Sprite2D.TextureAssetId)
            .Select(id => assetAtlas.TryGetRegistration(id, out AssetRegistration assetInstance) ? assetInstance.AssetId : id));
        init => _textureAssetIds = value;
    }
    
    public void ResetActiveEntityTree() {
        _activeEntityTree = null;
        _textureAssetIds = null;
    }
}
