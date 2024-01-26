
// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Assets;
using AterraEngine.Contracts.Assets;
using AterraEngine.Types;
using Raylib_cs;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PlayerControllerLooking:Player2D,IPlayer2D {
    public new Vector2 Velocity { get; set; } = new(2f, 2f);

    public PlayerControllerLooking(EngineAssetId engineAssetId) : base(engineAssetId, "playerLooking") {
        KeyMapping.Add( new KeyboardInput([KeyboardKey.Left]), (deltaTime) => { Rotation -= Velocity.X * 100 * deltaTime;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.Right]), (deltaTime) => { Rotation += Velocity.X * 100 * deltaTime;});
        KeyMapping.Add( new KeyboardInput([KeyboardKey.LeftAlt, KeyboardKey.P]), (deltaTime) => { Pos = new Vector2(0, 0);});
        
        KeyMapping.Add( new KeyboardInput([KeyboardKey.Up]), (deltaTime) => {  Size += Velocity * deltaTime; });
        KeyMapping.Add( new KeyboardInput([KeyboardKey.Down]), (deltaTime) => {Size -= Velocity * deltaTime; });
        
        OptionalKeyMapping.Add(
            new KeyboardInput([KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D, KeyboardKey.LeftShift]),
            (args, deltaTime) => Movement(deltaTime, args[0], args[1], args[2], args[3], args[4])    
        );
    }
    
    private void Movement(float deltaTime, bool isUpPressed, bool isDownPressed, bool isLeftPressed, bool isRightPressed, bool isRunning) {
        float horizontal = 0;
        float vertical = 0;
        if (isUpPressed) --vertical;
        if (isDownPressed) ++vertical;
        if (isLeftPressed) --horizontal;
        if (isRightPressed) ++horizontal;
        float speedMultiplier = isRunning ? 5 : 1;
        
        
        // Create the movement vector
        Vector2 movement = new Vector2(horizontal, vertical);
        // Normalize the movement vector
        if (movement != Vector2.Zero) movement = Vector2.Normalize(movement);

        
        // Apply the rotation to the movement
        movement = Vector2.Transform(movement, Matrix3x2.CreateRotation(Raylib.DEG2RAD * (Rotation + 90)));
        
        Pos += movement * Velocity * deltaTime * speedMultiplier;
        
    }
}