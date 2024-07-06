// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Nexities.Components;
using System.Numerics;

namespace AterraCore.Nexities.Lib.Components.Transform2D;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform2D>("Nexities:Components/Transform2D")]
public class Transform2D : NexitiesComponent, ITransform2D {
    public Vector2 Translation { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Rotation { get; set; } = Vector2.Zero;
}
