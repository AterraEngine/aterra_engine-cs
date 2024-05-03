// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Nexities.Lib.Components.Transform2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("0")]
[UsedImplicitly]
public class Transform2DComponent : Component, ITransform2DComponent {
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