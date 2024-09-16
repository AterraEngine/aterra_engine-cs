// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.World;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActiveLevel {
    INexitiesLevel RawLevelData { get; }
    LinkedList<INexitiesSystem> RawSystemData { get; }

    IEntityNodeTree ActiveEntityTree { get; }
    IRaylibCamera2D? Camera2DEntity { get; }

    FrozenSet<AssetId> TextureAssetIds { get; }

    bool TryGetRender2DSystems([NotNullWhen(true)] out IRender2DSystem[]? systems);
    bool TryGetRender3DSystems([NotNullWhen(true)] out IRender3DSystem[]? systems);
    bool TryGetRenderUiSystems([NotNullWhen(true)] out IRenderUiSystem[]? systems);
    bool TryGetLogicSystems([NotNullWhen(true)] out ILogicSystem[]? systems);

    bool TryGetRenderClearableCacheSystems([NotNullWhen(true)] out IRenderClearableCacheSystem[]? systems);
    bool TryGetLogicClearableCacheSystems([NotNullWhen(true)] out ILogicClearableCacheSystem[]? systems);
}
