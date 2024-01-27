// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Data;
using System.Numerics;
using AterraEngine.Contracts.Components;
using AterraEngine.DTO.Components;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Player2DKeyboardInputComponent<T> : AInputComponent<T> where T : Input2DDto, new() {
    private Vector2 Velocity { get; set; } = new(2f, 2f);
    private float SprintMultiplier { get; set; } = 2f;
    private float RotationMultiplier { get; set; } = 20f;
    private Vector2 SizeMultiplier { get; set; } = new(1f, 1f);
    
    public Player2DKeyboardInputComponent() {
        KeyMapping.Add(
            new KeyboardInput([KeyboardKey.LeftShift, KeyboardKey.RightShift], Handling.Any), 
            ProcessShift
        );
        
        KeyMapping.Add(
            new KeyboardInput([KeyboardKey.Left, KeyboardKey.Right], Handling.Any),
            ProcessRotation
        );
        
        KeyMapping.Add(
            new KeyboardInput([KeyboardKey.Up, KeyboardKey.Down], Handling.Any),
            ProcessSize
        );
        
        KeyMapping.Add(
            new KeyboardInput([KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D], Handling.Any),
            ProcessMovement
        );
    }

    private T ProcessRotation(T dto, bool[] args) {
        float[] factors = args.Select(b => b ? 1f : 0f).ToArray();
        dto.RotationOffset = RotationMultiplier * (factors[1] - factors[0]);
        return dto;
    }

    private T ProcessSize(T dto, bool[] args) {
        float[] factors = args.Select(b => b ? 1f : 0f).ToArray();
        dto.SizeOffset = SizeMultiplier * (factors[0] - factors[1]);
        return dto;
    }
    
    private T ProcessShift(T dto, bool[] args) {
        dto.SpeedMultiplier = SprintMultiplier;
        return dto;
    }
    
    private T ProcessMovement(T dto, bool[] args) {
        float[] factors = args.Select(b => b ? 1f : 0f).ToArray();
        Vector2 movement = new Vector2(
            (factors[3] - factors[2]),
            (factors[1] - factors[0])
        );

        
        // Normalize the movement vector
        if (movement != Vector2.Zero) movement = Vector2.Normalize(movement) * Velocity ;
            
        // Apply the rotation to the movement
        dto.DirectionOffset = movement;
        
        return dto;

    }
}