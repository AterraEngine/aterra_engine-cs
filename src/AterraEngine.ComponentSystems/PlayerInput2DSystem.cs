// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Components;
using AterraEngine.OldContracts.Systems;
using AterraEngine.DTO.Components;

namespace AterraEngine.ComponentSystems;

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