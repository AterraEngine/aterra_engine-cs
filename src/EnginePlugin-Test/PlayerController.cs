// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.structs;
using AterraEngine.Component;
using AterraEngine.Interfaces.Component;
using Raylib_cs;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PlayerController:Player2DComponent,IPlayerController {
    public new Vector2 Velocity { get; set; } = new(0.2f*100, 0.2f*100);

    public PlayerController() {
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_A]), () => { Pos = Pos with { X = Pos.X - Velocity.X };});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_D]), () => { Pos = Pos with { X = Pos.X + Velocity.X };});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_W]), () => { Pos = Pos with { Y = Pos.Y - Velocity.Y };});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_S]), () => { Pos = Pos with { Y = Pos.Y + Velocity.Y };});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_Q]), () => { Rotation -= 0.2f*100;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_E]), () => { Rotation += 0.2f*100;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_V, KeyboardKey.KEY_B]), () => {Console.WriteLine("BOTH ARE PRESSED");});
    }
}