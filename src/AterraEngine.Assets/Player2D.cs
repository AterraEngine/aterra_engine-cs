// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Player2D(EngineAssetId id, string? internalName): Actor(id, internalName), IPlayer2D {
    public Dictionary<KeyboardInput, Action<float>> KeyMapping { get; set; } = new();
    public Dictionary<KeyboardInput, Action<bool[], float>> OptionalKeyMapping { get; set; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void LoadKeyMapping(float deltaTime) {
        foreach ((KeyboardInput keyInput, Action<float> action) in KeyMapping) {
            if (keyInput.Keys.All(key => Raylib.IsKeyDown(key))) action(deltaTime);
        }
        
        foreach ((KeyboardInput keyInput, var action) in OptionalKeyMapping) {
            var enumerable = keyInput.Keys.Select(key => (bool)Raylib.IsKeyDown(key)).ToArray();
            if (enumerable.All(arg => !arg) || enumerable.Length != keyInput.Keys.Length) return; 
            action(enumerable, deltaTime);
        }
    }
}