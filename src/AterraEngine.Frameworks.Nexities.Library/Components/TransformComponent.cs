// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Frameworks.Nexities.Library.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class TransformComponent : NexitiesComponent<TransformComponent>, ITransformComponent {
    public Vector2 Position {
        get;
        set {
            field = value;
            Rectangle = new Rectangle(field, Scale);
        }
    } = Vector2.Zero;

    public Vector2 Scale {
        get;
        set {
            field = value;
            Rectangle = new Rectangle(Scale, field);
        }
    } = Vector2.One;

    public float Rotation {
        get;
        set => field = (value % 360f + 360f) % 360f;
    }

    public Rectangle Rectangle { get; private set; }
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