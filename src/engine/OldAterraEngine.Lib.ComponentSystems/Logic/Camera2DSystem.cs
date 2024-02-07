// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.ECS;
using OldAterraEngine.Contracts.ECS.EntityCombinations;
using OldAterraEngine.Contracts.WorldSpaces;
using OldAterraEngine.Core.ECS;
namespace OldAterraEngine.Lib.ComponentSystems.Logic;

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