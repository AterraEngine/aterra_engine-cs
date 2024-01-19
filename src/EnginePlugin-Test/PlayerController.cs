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
    public new Vector2 Velocity { get; set; } = new(1000f, 1000f);

    public PlayerController(EngineAssetId id) : base(id, "PLAYER") {
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_Q]), () => { Rotation -= Velocity.X*DeltaTime;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_E]), () => { Rotation += Velocity.X*DeltaTime;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_V, KeyboardKey.KEY_B]), () => {Console.WriteLine("BOTH ARE PRESSED");});
        
        OptionalKeyMapping.Add(
            new KeyboardInput([KeyboardKey.KEY_W, KeyboardKey.KEY_S, KeyboardKey.KEY_A, KeyboardKey.KEY_D, KeyboardKey.KEY_LEFT_SHIFT]),
            args => Movement(args[0], args[1], args[2], args[3], args[4])    
        );
    }

    public void Movement(bool isUpPressed, bool isDownPressed, bool isLeftPressed, bool isRightPressed, bool isRunning) {
        float horizontal = 0;
        float vertical = 0;
        if (isUpPressed) --vertical;
        if (isDownPressed) ++vertical;
        if (isLeftPressed) --horizontal; 
        if (isRightPressed) ++horizontal;

        Vector2 movement = new Vector2(horizontal, vertical);
        // Normalize the movement vector
        if (movement != Vector2.Zero) movement /= movement.Length();
        
        // Apply to position
        Pos += movement * Velocity * DeltaTime * (isRunning ? 2 : 1);
        
    }
}