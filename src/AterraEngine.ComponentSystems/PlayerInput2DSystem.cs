// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.Systems;
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
        
        Console.WriteLine(("PLayerInput2DSystem", movement.Direction, inputDto));
        
    }
}