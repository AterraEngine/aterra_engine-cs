// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace AterraEngine.DTO.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record Input2DDto {
    public Vector2 DirectionOffset = Vector2.Zero;
    public float SpeedMultiplier = 1f;
    public float RotationOffset = 0f;
    public Vector2 SizeOffset = Vector2.Zero;
    
    public Input2DDto() { }
}