// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.DTOs.Components;
using OldAterraEngine.Contracts.ECS.EntityCombinations;
using OldAterraEngine.Core.Types;
using OldAterraEngine.Lib.Components;
namespace OldAterraEngine.Lib.Actors.Entities;

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