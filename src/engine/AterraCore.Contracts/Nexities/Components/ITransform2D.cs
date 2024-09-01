// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITransform2D : INexitiesComponent {
    Vector2 Translation { get; set; }
    Vector2 Scale { get; set; }
    float Rotation { get; set; }
    Rectangle DestinationRectangle { get; }
    Vector2 RotationOrigin { get; }
}
