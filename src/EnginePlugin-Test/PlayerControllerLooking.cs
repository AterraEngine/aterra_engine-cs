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
public class PlayerControllerLooking:Player2DComponent,IPlayerController {
    public new Vector2 Velocity { get; set; } = new(1000f, 1000f);

    public PlayerControllerLooking() {
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_LEFT]), () => { Rotation -= (Velocity.X / 2)*DeltaTime;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_RIGHT]), () => { Rotation += (Velocity.X / 2)*DeltaTime;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_LEFT_ALT, KeyboardKey.KEY_P]), () => { Pos = new Vector2(0, 0);});
        
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_UP]), () => { Size *= 1.001f; });
        KeyMapping.Add( new KeyboardInput([KeyboardKey.KEY_DOWN]), () => {Size *= 0.999f; });
        
        OptionalKeyMapping.Add(
            new KeyboardInput([KeyboardKey.KEY_W, KeyboardKey.KEY_S, KeyboardKey.KEY_A, KeyboardKey.KEY_D, KeyboardKey.KEY_LEFT_SHIFT]),
            args => Movement(args[0], args[1], args[2], args[3], args[4])    
        );
    }

    private void Movement(bool isUpPressed, bool isDownPressed, bool isLeftPressed, bool isRightPressed, bool isRunning) {
        float horizontal = 0;
        float vertical = 0;
        if (isUpPressed) --vertical;
        if (isDownPressed) ++vertical;
        if (isLeftPressed) --horizontal;
        if (isRightPressed) ++horizontal;
        float speedMultiplier = isRunning ? 2 : 1;
        
        
        // Create the movement vector
        Vector2 movement = new Vector2(horizontal, vertical);
        // Normalize the movement vector
        if (movement != Vector2.Zero) movement = Vector2.Normalize(movement);

        
        // Apply the rotation to the movement
        movement = Vector2.Transform(movement, Matrix3x2.CreateRotation(Raylib.DEG2RAD * (Rotation + 90)));
        
        Pos += movement * Velocity * DeltaTime * speedMultiplier;
        
    }
}