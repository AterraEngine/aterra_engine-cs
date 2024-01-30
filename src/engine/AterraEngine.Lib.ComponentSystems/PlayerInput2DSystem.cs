// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Contracts.Systems;

namespace AterraEngine.Lib.ComponentSystems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PlayerInput2DSystem : ILogicSystem {
    public void Process(IAsset asset, float deltaTime) {
        if (!asset.TryGetComponent<IInputComponent<Input2DDto>>(out var input)) return;
        if (!asset.TryGetComponent<IMovement2DComponent>(out var movement)) return;

        Input2DDto inputDto = input.ProcessInput();
        movement.AssignFromInputDto(inputDto);
        
    }
}