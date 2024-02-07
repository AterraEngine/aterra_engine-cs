// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Contracts.ECS.EntityCombinations;
using AterraEngine.Core.Types;
using AterraEngine.Lib.Components;
namespace AterraEngine.Lib.Actors.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Player2D: Actor, IPlayer2D, IPlayer2DEntity {
    private IInputComponent<Input2DDto>? _input;
    public IInputComponent<Input2DDto> Input => _input ??= GetComponent<IInputComponent<Input2DDto>>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Player2D(EngineAssetId id, string? internalName) : base(id, internalName) {
        TryAddComponent<IInputComponent<Input2DDto>, Player2DKeyboardInputComponent<Input2DDto>>();
    }
}