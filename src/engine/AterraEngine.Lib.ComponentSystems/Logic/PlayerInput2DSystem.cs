// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.EntityCombinations;
using AterraEngine.Core.ECS;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PlayerInput2DSystem : EntityComponentSystem<IPlayer2DEntity> {
    
    public override void Update(IEntity e) {
        var entity = CastToEntity(e);
        Input2DDto inputDto = entity.Input.ProcessInput();
        entity.Movement.AssignFromInputDto(inputDto);
    }
}