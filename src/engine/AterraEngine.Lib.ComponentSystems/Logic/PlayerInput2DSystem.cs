// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Contracts.ECS.Logic;
namespace AterraEngine.Lib.ComponentSystems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PlayerInput2DSystem : ILogicSystem<IAsset> {
    public Type[] ComponentTypes { get; } = [
        typeof(IInputComponent<Input2DDto>),
        typeof(IMovement2DComponent)
    ];
    
    public void Process(IAsset asset, float deltaTime) {
        Input2DDto inputDto = asset.InputComponent.ProcessInput();
        asset.Movement2DComponent.AssignFromInputDto(inputDto);
    }
}