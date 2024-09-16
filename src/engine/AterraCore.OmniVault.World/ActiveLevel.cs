// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using AterraCore.Contracts.PoolCorps;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ActiveLevel(INexitiesSystemPools nexitiesSystemPools) : IActiveLevel {
    public required INexitiesLevel RawLevelData { get; init; }
    public required LinkedList<INexitiesSystem> RawSystemData { get; init; } = nexitiesSystemPools.LinkedListPool.Get();
    
    public required IEntityNodeTree ActiveEntityTree { get; init; }
    public required IRaylibCamera2D? Camera2DEntity { get; init; }
    public required FrozenSet<AssetId> TextureAssetIds { get; init; }
    
    private IRender2DSystem[]? _render2DSystems;
    private IRender3DSystem[]? _render3DSystems;
    private IRenderUiSystem[]? _renderUiSystems;
    private ILogicSystem[]? _logicSystems;
    private IRenderClearableCacheSystem[]? _renderClearableCacheSystems;
    private ILogicClearableCacheSystem[]? _logicClearableCacheSystems;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void ClearCaches() {
        _render2DSystems = null;
        _render3DSystems = null;
        _renderUiSystems = null;
        _logicSystems = null;
    }

    public bool Cleanup() {
        nexitiesSystemPools.LinkedListPool.Return(RawSystemData);
        return true;
    }

    private bool TryGetSystems<TSystemInterface>(ref TSystemInterface[]? cache, [NotNullWhen(true)] out TSystemInterface[]? systems) where TSystemInterface : INexitiesSystem {
        systems = default;
        if (cache is not null) {
            systems = cache;
            return true;
        }
        
        // Pool is cleaned up after everything
        using var listPool = new DisposablePooledResource<List<INexitiesSystem>>(nexitiesSystemPools.ListPool);
        List<INexitiesSystem> list = listPool.Item;
        
        foreach (INexitiesSystem nexitiesSystem in RawSystemData) {
            if (nexitiesSystem is TSystemInterface render2DSystem) {
                list.Add(render2DSystem);
            }
        }

        if (list.ToArray() is not TSystemInterface[] castedSystems) return false;
        systems = castedSystems;
        cache = castedSystems; // Because cache is a ref, we should be setting the original cache to the new thing.
        return true;
    }

    public bool TryGetRender2DSystems([NotNullWhen(true)] out IRender2DSystem[]? systems) => TryGetSystems(ref _render2DSystems, out systems);
    public bool TryGetRender3DSystems([NotNullWhen(true)] out IRender3DSystem[]? systems) => TryGetSystems(ref _render3DSystems, out systems);
    public bool TryGetRenderUiSystems([NotNullWhen(true)] out IRenderUiSystem[]? systems) => TryGetSystems(ref _renderUiSystems, out systems);
    public bool TryGetLogicSystems([NotNullWhen(true)] out ILogicSystem[]? systems) => TryGetSystems(ref _logicSystems, out systems);
    
    public bool TryGetRenderClearableCacheSystems([NotNullWhen(true)] out IRenderClearableCacheSystem[]? systems) => TryGetSystems(ref _renderClearableCacheSystems, out systems);
    public bool TryGetLogicClearableCacheSystems([NotNullWhen(true)] out ILogicClearableCacheSystem[]? systems) => TryGetSystems(ref _logicClearableCacheSystems, out systems);
}
