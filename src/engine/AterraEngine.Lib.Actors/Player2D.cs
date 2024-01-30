// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Core.Types;
using AterraEngine.Lib.Components;

namespace AterraEngine.Lib.Actors;

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