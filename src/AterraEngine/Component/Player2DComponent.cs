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
public class Player2DComponent(EngineAssetId id, string? internalName) : Actor2DComponent(id, internalName), IPlayerComponent {
    public Dictionary<KeyboardInput, Action> KeyMapping { get; set; } = new();
    public Dictionary<KeyboardInput, Action<bool[]>> OptionalKeyMapping { get; set; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void LoadKeyMapping() {
        foreach ((KeyboardInput keyInput, Action action) in KeyMapping) {
            if (keyInput.Keys.All(key => Raylib.IsKeyDown(key))) action();
        }
        
        foreach ((KeyboardInput keyInput, var action) in OptionalKeyMapping) {
            var enumerable = keyInput.Keys.Select(key => (bool)Raylib.IsKeyDown(key)).ToArray();
            if (enumerable.All(arg => !arg) || enumerable.Length != keyInput.Keys.Length) return; 
            action(enumerable);
        }
    }

    public float DeltaTime => Raylib.GetFrameTime();
}