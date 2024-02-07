// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.DTOs.Components;
using OldAterraEngine.Contracts.ECS;
using OldAterraEngine.Contracts.ECS.EntityCombinations;
using OldAterraEngine.Core.ECS;
namespace OldAterraEngine.Lib.ComponentSystems.Logic;

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