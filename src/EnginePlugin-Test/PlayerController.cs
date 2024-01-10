// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Component;
using AterraEngine.Interfaces.Component;
using Raylib_cs;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PlayerController:Player2DComponent,IPlayerController {
    public new Vector2 Velocity { get; set; } = new(0.2f, 0.2f);

    public PlayerController() {
        KeyMapping.Add(KeyboardKey.KEY_A, () => { Pos = Pos with { X = Pos.X - Velocity.X };});
        KeyMapping.Add(KeyboardKey.KEY_D, () => { Pos = Pos with { X = Pos.X + Velocity.X };});
        KeyMapping.Add(KeyboardKey.KEY_W, () => { Pos = Pos with { Y = Pos.Y - Velocity.Y };});
        KeyMapping.Add(KeyboardKey.KEY_S, () => { Pos = Pos with { Y = Pos.Y + Velocity.Y };});
        KeyMapping.Add(KeyboardKey.KEY_LEFT, () => { Rotation -= 0.1f;});
        KeyMapping.Add(KeyboardKey.KEY_RIGHT, () => { Rotation += 0.1f;});
        
        // KeyMapping.Add(KeyboardKey.KEY_UP, () => { this.Sprite.Size *= 1.001f;});
        // KeyMapping.Add(KeyboardKey.KEY_DOWN, () => { this.Sprite.Size *= .999f;});
    }
}