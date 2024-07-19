// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Contracts.Nexities.Data.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITransform2D : INexitiesComponent {
    Vector2 Translation { get; set; }
    Vector2 Scale { get; set; } 
    Vector2 Rotation { get; set; }
    Rectangle DestinationRectangle { get; }
}
