// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Frameworks.Nexities.Library.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class TransformComponent : NexitiesComponent<TransformComponent>, ITransformComponent {
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public float Rotation { get; set; } = 0f;

    public Rectangle Rectangle => new(Position.X, Position.Y, Scale.X, Scale.Y);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override bool TryReset() {
        if (!base.TryReset()) return false;
        Position = Vector2.Zero;
        Scale = Vector2.One;
        Rotation = 0f;
        return true;
    }
}