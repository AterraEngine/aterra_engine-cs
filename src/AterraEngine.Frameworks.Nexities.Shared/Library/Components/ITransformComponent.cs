// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Frameworks.Nexities.Library.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITransformComponent : INexitiesComponent {
    Vector2 Position { get; set; }
    Vector2 Scale { get; set; } 
    float Rotation { get; set; }
    
    Rectangle Rectangle { get; }
}