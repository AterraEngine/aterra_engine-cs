// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Components;
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Components;
using AterraEngine.DTO.Components;
using AterraEngine.Types;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Player2D: Actor, IPlayer2D {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public Player2D(EngineAssetId id, string? internalName) : base(id, internalName) {
        TryAddComponent<IInputComponent<Input2DDto>, Player2DKeyboardInputComponent<Input2DDto>>();
    }
}