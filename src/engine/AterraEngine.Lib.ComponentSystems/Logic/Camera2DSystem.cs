// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.EntityCombinations;
using AterraEngine.Contracts.WorldSpaces;
using AterraEngine.Core.ECS;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Camera2DSystem(IWorldSpace2D worldSpace2D) : EntityComponentSystem<ICamera2D> {
    public override void Update(IEntity e) {
        var camera = CastToEntity(e);
        IPlayer2DEntity player = worldSpace2D.LoadedLevel!.GetPlayer();
        
        camera.Camera2DComponent.UpdateCamera(player.Transform.Pos, worldSpace2D.DeltaTime);
    }
}