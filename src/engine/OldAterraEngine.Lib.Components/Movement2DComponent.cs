// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.DTOs.Components;

namespace OldAterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Movement2DComponent : IMovement2DComponent {
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public float RotationOffset { get; set; }
    public Vector2 SizeOffset { get; set; }
    
    public void AssignFromInputDto<T>(T dto) where T : Input2DDto {
        this.Direction = dto.DirectionOffset;
        this.Speed = dto.SpeedMultiplier;
        this.RotationOffset = dto.RotationOffset;
        this.SizeOffset = dto.SizeOffset;
    }

    public bool HasMovement() {
        return Direction != Vector2.Zero 
               && Math.Abs(Speed - 1) < 0.01f 
               && RotationOffset != 0;
    }
}