// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Core.ECSFramework;
namespace AterraEngine.Contracts.Lib.ECS.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITransform2D : IComponent {
    float Rotation { get; set; }
    Vector2 Position { get; set; }
    Vector2 Scale { get; set; }
    Matrix3x2 TransformationMatrix { get; }
}