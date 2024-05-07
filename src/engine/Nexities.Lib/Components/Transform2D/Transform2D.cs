// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Nexities.Components;

namespace Nexities.Lib.Components.Transform2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform2D>("AC000000")]
public class Transform2D : NexitiesComponent, ITransform2D {
    public Vector2 Translation { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Rotation { get; set; } = Vector2.Zero;
    
    // public override bool TryReset() {
    //     Translation = Vector2.Zero;
    //     Scale = Vector2.One;
    //     Rotation = Vector2.Zero;
    //
    //     return true;
    // }
}