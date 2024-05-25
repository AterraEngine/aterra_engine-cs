// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace Nexities.Lib.Components.Transform2D;

using AterraCore.Nexities.Data.Components;
using AterraCore.Nexities.DataParsing.NamedValues.PreMade;

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