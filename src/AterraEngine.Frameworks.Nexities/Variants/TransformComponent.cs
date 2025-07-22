// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Frameworks.Nexities.Variants;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class TransformComponent : NexitiesComponent {
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public float Rotation { get; set; } = 0f;
    
    public Rectangle Rectangle => new(Position.X, Position.Y, Scale.X, Scale.Y);
}