// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using AterraCore.DI;
using Raylib_cs;

namespace AterraCore.OmniVault.World;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct ActiveLevel : IActiveLevel {
    public INexitiesLevel2D RawLevelData { get; init; }
    public IReadOnlyCollection<INexitiesSystem> LogicSystems { get; init; }
    public IReadOnlyCollection<INexitiesSystem> RenderSystems { get; init; }
    public IEntityNodeTree ActiveEntityTree { get; init; }
    
    private Ulid? _camera2D;
    public Camera2D Camera {
        get {
            Ulid id = _camera2D ??= ((ICamera2D)ActiveEntityTree.GetAsFlat().First(asset => asset is ICamera2D))
                .RaylibCamera2D.InstanceId;
            EngineServices.GetService<IAssetInstanceAtlas>().TryGet(id, out IRaylibCamera2D? camera2D);
            return camera2D?.Camera ?? new Camera2D();
         }
    }
}
