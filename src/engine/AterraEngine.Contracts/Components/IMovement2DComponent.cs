// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.DTOs.Components;

namespace AterraEngine.Contracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IMovement2DComponent : IComponent{
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public float RotationOffset { get; set; }
    public Vector2 SizeOffset { get; set; }

    public bool HasMovement();
    public void AssignFromInputDto<T>(T dto) where T: Input2DDto;
}