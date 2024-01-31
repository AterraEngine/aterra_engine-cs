// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Logic;
using AterraEngine.Core.ECS.Logic;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PlayerInput2DSystem : LogicSystem<IPlayer2D> {
    public override Type[] ComponentTypes { get; } = [
        typeof(IInputComponent<Input2DDto>),
        typeof(IMovement2DComponent)
    ];
    
    public override void Process(IEntity entity, float deltaTime) {
        IPlayer2D player = ConvertEntity(entity);
        
        Input2DDto inputDto = player.Input.ProcessInput();
        player.Movement.AssignFromInputDto(inputDto);
    }
}