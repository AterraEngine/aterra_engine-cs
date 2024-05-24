// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.SaveFileSystems.NamedValues.PreMade;
namespace Nexities.Lib.Components.Transform2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform2D>("AC000000")]
public class Transform2D : NexitiesComponent, ITransform2D {

    [NamedValueVector2("Translation")]
    public Vector2 Translation { get; set; } = Vector2.Zero;

    [NamedValueVector2("Scale")]
    public Vector2 Scale { get; set; } = Vector2.One;

    [NamedValueVector2("Rotation")]
    public Vector2 Rotation { get; set; } = Vector2.Zero;
}