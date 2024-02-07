// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace AterraEngine.Contracts.DTOs.Components;

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