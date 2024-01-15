// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Component;
using Raylib_cs;

namespace AterraEngine.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Player2DComponent : ActorComponent, IPlayerComponent {
    public Dictionary<KeyboardInput, Action> KeyMapping { get; set; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void LoadKeyMapping() {
        foreach ((KeyboardInput keyInput, Action action) in KeyMapping) {
            if (keyInput.Keys.All(key => Raylib.IsKeyDown(key))) action();
        }
    }
    
}